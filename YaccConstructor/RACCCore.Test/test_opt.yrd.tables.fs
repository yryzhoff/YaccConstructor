//this tables was generated by RACC
//source grammar:..\Tests\RACC\test_opt\test_opt.yrd
//date:3/27/2011 9:02:51 PM

#light "off"
module Yard.Generators.RACCGenerator.Tables_Opt

open Yard.Generators.RACCGenerator

type symbol =
    | T_PLUS
    | T_NUMBER
    | NT_s
    | NT_raccStart
let getTag smb =
    match smb with
    | T_PLUS -> 3
    | T_NUMBER -> 2
    | NT_s -> 1
    | NT_raccStart -> 0
let getName tag =
    match tag with
    | 3 -> T_PLUS
    | 2 -> T_NUMBER
    | 1 -> NT_s
    | 0 -> NT_raccStart
    | _ -> failwith "getName: bad tag."
let private autumataDict = 
dict [|(0,{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol 1);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbS 0))|]
|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 0))|]
|];
   ToStateID   = 2;
}
|];
}
);(1,{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,(State 2));(3,DummyState);(4,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1;2|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol 2);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 5));(FATrace (TSmbS 1))|]
|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = (DSymbol 3);
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TOptS 2));(FATrace (TOptE 2));(FATrace (TSeqE 5))|]
;List.ofArray [|(FATrace (TSmbE 1));(FATrace (TOptS 2));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|]
|];
   ToStateID   = 2;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TOptS 2));(FATrace (TOptE 2));(FATrace (TSeqE 5))|]
;List.ofArray [|(FATrace (TSmbE 1));(FATrace (TOptS 2));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|]
|];
   ToStateID   = 3;
}
;{ 
   FromStateID = 2;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TOptE 2));(FATrace (TSeqE 5))|]
|];
   ToStateID   = 4;
}
|];
}
)|]

let private gotoSet = 
    Set.ofArray [|((0, 0, 1), set [(0, 1)]);((0, 0, 2), set [(1, 1)]);((1, 0, 2), set [(1, 1)]);((1, 1, 3), set [(1, 2)])|]
    |> dict
let tables = { gotoSet = gotoSet; automataDict = autumataDict }

