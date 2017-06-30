namespace VaughanTests
    module ScalesHormonizerTests =
        open NUnit.Framework
        open FsCheck
        open FsCheck.NUnit
        open Swensen.Unquote
        open Vaughan.Domain
        open Vaughan.ScaleHarmonizer
        open Vaughan.Scales
        open Vaughan.Notes

        let chord = {Notes= []; ChordType=Closed; Name=""}
        let cMaj = {chord with Notes= [(C, Root); (E, Third); (G, Fifth)]}
        let dMin = {chord with Notes= [(D, Root); (F, Third); (A, Fifth)]}
        let eMin = {chord with Notes= [(E, Root); (G, Third); (B, Fifth)]}
        let fMaj = {chord with Notes= [(F, Root); (A, Third); (C, Fifth)]}
        let gMaj = {chord with Notes= [(G, Root); (B, Third); (D, Fifth)]}
        let aMin = {chord with Notes= [(A, Root); (C, Third); (E, Fifth)]}
        let bDim = {chord with Notes= [(B, Root); (D, Third); (F, Fifth)]}
        let cMin = {chord with Notes= [(C, Root); (EFlat, Third); (G, Fifth)]}
        let dDim = {chord with Notes= [(D, Root); (F, Third); (AFlat, Fifth)]}
        let eFlatAug = {chord with Notes= [(EFlat, Root); (G, Third); (B, Fifth)]}
        let fMin = {chord with Notes= [(F, Root); (AFlat, Third); (C, Fifth)]}
        let aFlatMaj = {chord with Notes= [(AFlat, Root); (C, Third); (EFlat, Fifth)]}
        let cMaj7 = {chord with Notes= [(C, Root); (E, Third); (G, Fifth); (B, Seventh)]}
        let dMin7 = {chord with Notes= [(D, Root); (F, Third); (A, Fifth); (C, Seventh)]}
        let eMin7 = {chord with Notes= [(E, Root); (G, Third); (B, Fifth); (D, Seventh)]}
        let fMaj7 = {chord with Notes= [(F, Root); (A, Third); (C, Fifth); (E, Seventh)]}
        let gDom7 = {chord with Notes= [(G, Root); (B, Third); (D, Fifth); (F, Seventh)]}
        let aMin7 = {chord with Notes= [(A, Root); (C, Third); (E, Fifth); (G, Seventh)]}
        let bMin7b5 = {chord with Notes= [(B, Root); (D, Third); (F, Fifth); (A, Seventh)]}
        let cMaj9 = {chord with Notes= [(C, Root); (E, Third); (G, Fifth); (B, Seventh); (D, Ninth)]}
        let dMin9 = {chord with Notes= [(D, Root); (F, Third); (A, Fifth); (C, Seventh); (E, Ninth)]}
        let eMin9 = {chord with Notes= [(E, Root); (G, Third); (B, Fifth); (D, Seventh); (F, Ninth)]}
        let fMaj9 = {chord with Notes= [(F, Root); (A, Third); (C, Fifth); (E, Seventh); (G, Ninth)]}
        let gDom9 = {chord with Notes= [(G, Root); (B, Third); (D, Fifth); (F, Seventh); (A, Ninth)]}
        let aMin9 = {chord with Notes= [(A, Root); (C, Third); (E, Fifth); (G, Seventh); (B, Ninth)]}
        let bMin9b5 = {chord with Notes= [(B, Root); (D, Third); (F, Fifth); (A, Seventh); (C, Ninth)]}
        let cMinMaj7 = {chord with Notes= [(C, Root); (EFlat, Third); (G, Fifth); (B, Seventh)]}
        let dMin7b5 = {chord with Notes= [(D, Root); (F, Third); (AFlat, Fifth); (C, Seventh)]}
        let eFlatAug7 = {chord with Notes= [(EFlat, Root); (G, Third); (B, Fifth); (D, Seventh)]}
        let fMin7 = {chord with Notes= [(F, Root); (AFlat, Third); (C, Fifth); (EFlat, Seventh)]}
        let aFlatMaj7 = {chord with Notes= [(AFlat, Root); (C, Third); (EFlat, Fifth); (G, Seventh)]}
        let bDim7 = {chord with Notes= [(B, Root); (D, Third); (F, Fifth); (AFlat, Seventh)]}

        [<Test>]
        let ``Should create triads for Ionian scale`` () =
            let cIonian = createScale Ionian C
            (triadsHarmonizer ScaleDegrees.I cIonian).Notes =! cMaj.Notes
            (triadsHarmonizer ScaleDegrees.II cIonian).Notes =! dMin.Notes
            (triadsHarmonizer ScaleDegrees.III cIonian).Notes =! eMin.Notes
            (triadsHarmonizer ScaleDegrees.IV cIonian).Notes =! fMaj.Notes
            (triadsHarmonizer ScaleDegrees.V cIonian).Notes =! gMaj.Notes
            (triadsHarmonizer ScaleDegrees.VI cIonian).Notes =! aMin.Notes
            (triadsHarmonizer ScaleDegrees.VII cIonian).Notes =! bDim.Notes

        [<Test>]
        let ``Should create triads for Harmonic Minor scale`` () =
            let cMinor = createScale HarmonicMinor C
            (triadsHarmonizer ScaleDegrees.I cMinor).Notes =! cMin.Notes
            (triadsHarmonizer ScaleDegrees.II cMinor).Notes =! dDim.Notes
            (triadsHarmonizer ScaleDegrees.III cMinor).Notes =! eFlatAug.Notes
            (triadsHarmonizer ScaleDegrees.IV cMinor).Notes =! fMin.Notes
            (triadsHarmonizer ScaleDegrees.V cMinor).Notes =! gMaj.Notes
            (triadsHarmonizer ScaleDegrees.VI cMinor).Notes =! aFlatMaj.Notes
            (triadsHarmonizer ScaleDegrees.VII cMinor).Notes =! bDim.Notes

        [<Property>]
        let ``Should create triads for scale`` (scaleType: Scale) (scaleDegree: ScaleDegrees) (root: Note)=
            (scaleType <> Blues && scaleType <> MajorPentatonic && scaleType <> MinorPentatonic && scaleType <> Bebop && scaleType <> NeapolitanMinor)
                ==> lazy (
                    let scale = createScale scaleType root

                    (triadsHarmonizer scaleDegree scale).Notes
                    |> List.pairwise
                    |> List.map (fun e -> intervalBetween (fst (fst e)) (fst (snd e)))
                    |> List.forall (
                        fun e -> e = MajorThird || e = MinorThird || e = MajorSecond || e = PerfectFourth))

        [<Test>]
        let ``Should create seventh chords for Ionian scale`` () =
            let cIonian = createScale Ionian C
            (seventhsHarmonizer ScaleDegrees.I cIonian).Notes =! cMaj7.Notes
            (seventhsHarmonizer ScaleDegrees.II cIonian).Notes =! dMin7.Notes
            (seventhsHarmonizer ScaleDegrees.III cIonian).Notes =! eMin7.Notes
            (seventhsHarmonizer ScaleDegrees.IV cIonian).Notes =! fMaj7.Notes
            (seventhsHarmonizer ScaleDegrees.V cIonian).Notes =! gDom7.Notes
            (seventhsHarmonizer ScaleDegrees.VI cIonian).Notes =! aMin7.Notes
            (seventhsHarmonizer ScaleDegrees.VII cIonian).Notes =! bMin7b5.Notes

        [<Test>]
        let ``Should create seventh chords for Harmonic Minor scale`` () =
            let cMinor = createScale HarmonicMinor C
            (seventhsHarmonizer ScaleDegrees.I cMinor).Notes =! cMinMaj7.Notes
            (seventhsHarmonizer ScaleDegrees.II cMinor).Notes =! dMin7b5.Notes
            (seventhsHarmonizer ScaleDegrees.III cMinor).Notes =! eFlatAug7.Notes
            (seventhsHarmonizer ScaleDegrees.IV cMinor).Notes =! fMin7.Notes
            (seventhsHarmonizer ScaleDegrees.V cMinor).Notes =! gDom7.Notes
            (seventhsHarmonizer ScaleDegrees.VI cMinor).Notes =! aFlatMaj7.Notes
            (seventhsHarmonizer ScaleDegrees.VII cMinor).Notes =! bDim7.Notes

        [<Property>]
        let ``Should create seventh chords for scale`` (scaleType: Scale) (scaleDegree: ScaleDegrees) (root: Note) =
            (scaleType <> Blues && scaleType <> MajorPentatonic && scaleType <> MinorPentatonic
                && scaleType <> WholeTone && scaleType <> Bebop && scaleType <> NeapolitanMinor)
                ==> lazy (
                    let scale = createScale scaleType root

                    (seventhsHarmonizer scaleDegree scale).Notes
                    |> List.pairwise
                    |> List.map (fun e -> intervalBetween (fst (fst e)) (fst (snd e)))
                    |> List.forall (
                        fun e -> e = MajorThird || e = MinorThird || e = MajorSecond || e = PerfectFourth))

        [<Test>]
        let ``Should create ninth chords for Ionian scale`` () =
            let cIonian = createScale Ionian C
            (ninthsHarmonizer ScaleDegrees.I cIonian).Notes =! cMaj9.Notes
            (ninthsHarmonizer ScaleDegrees.II cIonian).Notes =! dMin9.Notes
            (ninthsHarmonizer ScaleDegrees.III cIonian).Notes =! eMin9.Notes
            (ninthsHarmonizer ScaleDegrees.IV cIonian).Notes =! fMaj9.Notes
            (ninthsHarmonizer ScaleDegrees.V cIonian).Notes =! gDom9.Notes
            (ninthsHarmonizer ScaleDegrees.VI cIonian).Notes =! aMin9.Notes
            (ninthsHarmonizer ScaleDegrees.VII cIonian).Notes =! bMin9b5.Notes