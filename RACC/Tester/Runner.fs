﻿// Tester.fs
//
// Copyright 2009-2010 Semen Grigorev
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation.


open Yard.Core

let  runTest testFilePath = 
    let commandLineArgs = System.Environment.GetCommandLineArgs()  
    //We have some problems with serialization/seserialization.      
    let gotoSet,items,startNTerms,ruleToActionMap = Generator.generate (Main.ParseFile testFilePath)
    let tables = new TablesLoader(testFilePath, gotoSet, items, startNTerms, ruleToActionMap)
    let parser = new Parser(tables)
    
    //now we have not lexer. Lists from Test module are emulation of input stream
    let inputStream = Test.test011_2
    let trees = parser.Run inputStream        
    Seq.iter(fun b -> AST.PrintTree b) trees
    printfn "Parser get %A dirivation tree" trees.Length
    let astInterp = new ASTInterpretator(tables)    
    let res_2 = List.map astInterp.Interp trees
    List.iter (printf "\nresult: %A") res_2
    
do runTest @"..\..\..\..\Tests\test011.yrd"