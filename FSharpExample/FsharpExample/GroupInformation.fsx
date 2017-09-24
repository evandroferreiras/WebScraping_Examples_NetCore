#r "packages/FSharp.Data/lib/net40/FSharp.Data.dll"

open FSharp.Data
open System
open System.Text.RegularExpressions

let IsYes (v:string) = Regex(@"(Yes\[.*\]|Yes)").Replace(v,"Yes")

let RemoveLast sq = 
    sq |> Seq.rev |> Seq.tail

type LangCodes = HtmlProvider<"https://en.wikipedia.org/wiki/Comparison_of_programming_languages">
let GeneralComparison = LangCodes().Tables.``General comparison``
let rows = GeneralComparison.Rows
let GroupedResult = rows |> RemoveLast |> Seq.groupBy (fun x -> x.Functional |> IsYes )
let Summary = GroupedResult |> Seq.map (fun (x,y) -> x, y |> Seq.length )

printfn "%A" Summary
