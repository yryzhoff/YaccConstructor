﻿//namespace VSYardNS
//
//open System.Collections.Generic
//open System
//open System.ComponentModel.Composition
//open Microsoft.VisualStudio.Text
//open Microsoft.VisualStudio.Text.Classification
//open Microsoft.VisualStudio.Text.Editor
//open Microsoft.VisualStudio.Text.Tagging
//open Microsoft.VisualStudio.Utilities
//open System.Linq
//open Yard.Frontends.YardFrontend.Main
//open Yard.Frontends.YardFrontend.GrammarParser
//open Microsoft.VisualStudio.Language.Intellisense
//open Yard.Core.Checkers
//open System.Collections.Concurrent
//
//[<Export(typeof<ICompletionSourceProvider>)>]
//[<ContentType("text")>]
//[<Name("YardCompletion")>]
//
//type YardCompletionSource (buffer : ITextBuffer) =
//    let mutable _buffer : ITextBuffer = null
//    let mutable _disposed = false
//    let mutable theList = new Concurrent(new List<Completion>())
//    do
//        _buffer <- buffer
//    let augmentCompletionSession (session : ICompletionSession) (completionSets : IList<CompletionSet>) = 
//        //there was a check
//        let fileText = _buffer.CurrentSnapshot.GetText()              
//        //get completions
//        let getNonterminals (tree : Yard.Core.IL.Definition.t<Yard.Core.IL.Source.t,Yard.Core.IL.Source.t> ) = 
//            tree.grammar |> List.map (fun node -> new Completion(node.name))
//        let completions =
//            try 
//                let parsed = ParseText fileText
//                let getText (completion : Completion) = completion.DisplayText
//                let sorted = List.sortBy getText (getNonterminals parsed)
//                sorted.Distinct()
//            with
//            | _ -> Seq.empty
//        let snapshot = _buffer.CurrentSnapshot
//        let triggerPoint = session.GetTriggerPoint(snapshot).GetValueOrDefault() //ok?
//        //check that triggerPoint != null
//        let line = triggerPoint.GetContainingLine()
//        let start = ref triggerPoint
//        while (!start).Position > line.Start.Position  && not ( Char.IsWhiteSpace((!start - 1).GetChar()) ) do //why doesn't work without impicit getting POSITION?
//            start := !start - 1
//        //let applicableTo = snapshot.CreateTrackingSpan(new SnapshotSpan(!start, triggerPoint) :> Span, SpanTrackingMode.EdgeInclusive)//wtf? it works in C#
//        let x = new SnapshotSpan(!start, triggerPoint)
//        let applicableTo = snapshot.CreateTrackingSpan(x.Span, SpanTrackingMode.EdgeInclusive)//wtf? it works in C#;
//        completionSets.Add(new CompletionSet("All", "All", applicableTo, completions, Enumerable.Empty<Completion>()))
//
//    let dispose () = 
//        _disposed <- true
//    interface ICompletionSource with
//        member self.AugmentCompletionSession (x, y) = augmentCompletionSession x y
//        member self.Dispose () = dispose ()
//
//type YardCompletionSourceProvider ( textBuffer : ITextBuffer) = 
//    interface ICompletionSourceProvider with
//     member self.TryCreateCompletionSource textBuffer = 
//        new YardCompletionSource(textBuffer) :> ICompletionSource
