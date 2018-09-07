namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module TeXInput =
    open WebSharper.MathJax

    type MainTemplate = Templating.Template<"wwwroot/index.html">

    [<SPAEntryPoint>]
    let Main () =
        MathJax.Hub.Config(
            MathJax.Config(
                Extensions = [| "tex2jax.js" |],
                Jax = [| "input/TeX"; "output/HTML-CSS"; |],
                Tex2jax = MathJax.Tex2jax(InlineMath = [| ("$", "$"); ("\\(", "\\)") |])
            )
        )

        let rvExpression = Var.Create @"\frac{2\cdot x\cdot\left({ x}^{9}+{ x}^{2}\right)-{ x}^{2}\cdot\left(9\cdot{ x}^{8}+2\cdot x\right)}{{\left({ x}^{9}+{ x}^{2}\right)}^{2}}+2\cdot{ x}^{3}"

        let viewExpression = rvExpression.View

        let tex = 
            div [
                on.viewUpdate viewExpression (fun e v -> 
                    JQuery.Of("#tex div").Empty().Append("$$" + v + "$$").Ignore
                    MathJax.Hub.Queue([| "Typeset", MathJax.MathJax.Hub :> obj, [| e :> obj |] |]) |> ignore
                )
            ] [
                textView <| viewExpression.Map (fun x -> "$$" + x + "$$")
            ]

        Doc.RunById "tex" tex

        MainTemplate.Main()
            .Expression(rvExpression)
            .Doc()
        |> Doc.RunById "main"
