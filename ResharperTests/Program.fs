﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open NUnit.Framework
open YC.Bio.RNA.SearchFSA

[<TestFixture>]
type ``Bio Test`` () =

//    [<Test>]
//    member test.``Strange Error``() =
//        //runTest "BadLeftRecursion.yrd" "BBB.txt"
//        checkAst "code.yrd" "code.txt"
//            19 24 3 1
    [<Test>]
    member test.``Main``() =
        //runTest "BadLeftRecursion.yrd" "BBB.txt"
        main [|"--input"; "../../../tests/data/bio/synth_for_semen/simplification"|]

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
