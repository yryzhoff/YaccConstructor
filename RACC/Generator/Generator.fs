﻿// Generator.fs
//
// Copyright 2009-2010 Semen Grigorev
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation.

module Yard.Core.Generator

open IL.Production
open IL.Rule
open IL
open Grammar.Item
open GrammarPreparer


let start_time = ref System.DateTime.Now
let end_time   = ref System.DateTime.Now      

let items,_grammar,_generate, ruleToActionMap=
    let _items = ref Set.empty
    let _grammar = ref[]
    let _ruleToActonMap = ref[]
    let generate rules path =
       let codeGenerator = new CodeGenerator(path,path+".fs")
       codeGenerator.Write (codeGenerator.GenHeader())
       codeGenerator.Write (codeGenerator.GenDefaultFunctions)
       let finitAutomata = new FinitAutomataCreator(codeGenerator) 
       _grammar := rules;
       let rules_map  = List.zip ([0..(List.length rules)-1])rules
       let creatItem (i,rule) =
          let (itm,s:FAState,f:Set<FAState>),code,binding = finitAutomata.FA_rules rule.body
          let topLevelBindingName = rule.name+i.ToString()+"_action"
          _ruleToActonMap:=(i,topLevelBindingName)::(!_ruleToActonMap)
          if rule.name<>"_yard_start" 
          then codeGenerator.Write (codeGenerator.GenTopLEvelBinding topLevelBindingName code binding)
          let get_symb = function 
                           Some((PLiteral(s)|PToken(s)|PRef(s,_)),_) -> Some(Source.toString s)                                                                                  
                           | _ -> failwith "Generator error." 
          let getSeqNum = function 
                           Some(_,seqNum) -> seqNum                                                                                  
                           | _ -> failwith "Generator error. Can not find seqNumber"                                                                          
          #if DEBUG
          Log.print_item itm s f;
          #endif
          let _createItem buf (fromS:FAState,symbol,toS:FAState) =
              let newItem  itemNum nextNum fromSTrace toSTrace=
                {prod_num = i;
                 prod_name = rule.name;
                 item_num = itemNum;
                 symb = get_symb symbol;
                 next_num = nextNum;
                 seq_number = getSeqNum symbol;
                 s = s.num;
                 f = Set.map (fun (_f:FAState) -> _f.num) f;
                 fromStateTrace = fromSTrace;
                 toStateTrace = toSTrace
                }
              buf + Set.singleton(newItem fromS.num (Some(toS.num)) fromS.trace toS.trace)+
               if Set.exists ((=)toS.num) (Set.map (fun (_f:FAState) -> _f.num) f)
               then Set.singleton(newItem toS.num None fromS.trace toS.trace)
               else Set.empty 
               
          Set.fold _createItem Set.empty itm
       _items := List.map creatItem rules_map |> Set.unionMany;
       codeGenerator.CloseOutStream() 
    let items () = !_items
    let grammar () = !_grammar
    let ruleToActionMap ()= dict !_ruleToActonMap
    items,grammar,generate,ruleToActionMap
    
let closure q = 
    let q' = ref (set q)
    let l = ref 0
    while (!l < Set.count !q') do
        l:= Set.count !q';
        for item in !q' 
            do for item' in (items()) 
                   do if Option.get item.symb = item'.prod_name && item'.item_num = 0
                      then q':= Set.add item' !q'
        
    !q'              
        
let get_closure_set,calc_closure_set = 
#if DEBUG
  Log.print_items (items())
#endif
  let _closure_set = ref( dict []);
  let calculate_clousure_set () =
      _closure_set := dict <| Set.map (fun x -> x, closure (Set.singleton x)) (items())                
  let closure_set () = !_closure_set
  closure_set,calculate_clousure_set

let goto_set ()=         
    let make_goto q x =  
        calc_closure_set()
        let closure = Set.fold (fun y x -> y + get_closure_set().[x]) Set.empty q
        Set.unionMany 
          <|seq {for item in closure do 
                  if x = Option.get item.symb 
                  then yield Utils.nextItem item (items())}
    let toString = function | PToken y |PLiteral y | PRef (y,_) -> Source.toString y 
                            | _ -> ""
    let goto_data symbol item = 
        let gt = make_goto (Set.singleton item) symbol
#if DEBUG        
        printf "\n GOTO \n:";
        Log.print_goto_c symbol item gt;
#endif        
        hash(item, symbol),gt
    dict <| Set.fold (fun buf symbol -> buf@[for item in (items()) -> goto_data symbol item]) 
                      [] (GrammarPreparer.get_all_t(_grammar()))
                       
let generate input_grammar = 
    let head,rules,foot = GrammarPreparer.prepare input_grammar
    let addStartRule rules = 
        List.fold (fun rules rule_name -> (GrammarPreparer.createStartRule "_yard_start" rule_name)::rules)
                  (replace_Public rules) (GrammarPreparer.get_start_nterms rules)
    _generate(ExpandMeta.expandMetaRules (addStartRule rules))(input_grammar.info.fileName);
    
#if DEBUG    
    printf "Transformed grammar \n %A\n" <|_grammar()
    printf "\n Token list: \n  ";
    Set.iter (printf "%A;")(GrammarPreparer.get_all_t(_grammar()))
    printf "\n Start Nterms: \n %A " <|GrammarPreparer.get_start_nterms (_grammar())
#endif

    let gotoSet = goto_set()
    let items = items()
    let startNTerms = GrammarPreparer.get_start_nterms (_grammar())
    let ruleToActionMap = ruleToActionMap()
    IO.writeValue (input_grammar.info.fileName + ".goto.dta") (System.Linq.Enumerable.ToList(gotoSet ))
    IO.writeValue (input_grammar.info.fileName + ".items.dta") items
    IO.writeValue (input_grammar.info.fileName + ".start_nterms.dta") startNTerms
    IO.writeValue (input_grammar.info.fileName + ".rule_to_action.dta") (System.Linq.Enumerable.ToList(ruleToActionMap))
    printfn "End working time: %A Total: %A" System.DateTime.Now (System.DateTime.Now - (!start_time))
    (gotoSet,items,startNTerms,ruleToActionMap)