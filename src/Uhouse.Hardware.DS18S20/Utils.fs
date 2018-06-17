module Utils

let rec isPrefixOf pref li = match pref, li with
    | [], _ -> true
    | x::xs, y::ys -> x = y && isPrefixOf xs ys

let s_isPrefixOf (pref : string) (li : string) = isPrefixOf (Seq.toList pref) (Seq.toList li)

let (|SeqEmpty|SeqCons|) (xs: 'a seq) =
  if Seq.isEmpty xs then SeqEmpty
  else SeqCons(Seq.head xs, Seq.skip 1 xs)

let rec stripPrefix pref str = match pref, str with
    | SeqEmpty, str -> Some str
    | SeqCons (x,xs), SeqCons (y,ys) when x = y -> stripPrefix xs ys
    | _,_ -> None

let words (s:string) = s.Split (' ', '\n') |> Array.toList

let rec asum (xs : 'a option list) : 'a option =
    match xs with
    | [] -> None
    | Some x::_ -> Some x
    | _::xs -> asum xs