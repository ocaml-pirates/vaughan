namespace VaughanTests
    module ScaleTests =
        open NUnit.Framework
        open FsCheck
        open FsCheck.NUnit
        open FsUnit
        open Vaughan.Domain
        open Vaughan.Scales
        open Vaughan.Chords

        [<Test>]
        let ``Should have notes for scales``() =
            createScale Ionian C |> should equal [ C; D; E; F; G; A; B ]
            createScale Dorian C |> should equal [ C; D; EFlat; F; G; A; BFlat ]
            createScale Phrygian C |> should equal [ C; DFlat; EFlat; F; G; AFlat; BFlat ]
            createScale Lydian C |> should equal [ C; D; E; FSharp; G; A; B ]
            createScale Mixolydian C |> should equal [ C; D; E; F; G; A; BFlat ]
            createScale Aolian C |> should equal [ C; D; EFlat; F; G; AFlat; BFlat ]
            createScale Locrian C |> should equal [ C; DFlat; EFlat; F; GFlat; AFlat; BFlat ]
            createScale MajorPentatonic C |> should equal [ C; D; E; G; A;]
            createScale MinorPentatonic C |> should equal [ C; EFlat; F; G; BFlat ]
            createScale Blues C |> should equal [ C; EFlat; F; GFlat; G; BFlat ]
            createScale HarmonicMinor C |> should equal [ C; D; EFlat; F; G; AFlat; B ]
            createScale MelodicMinor C |> should equal [ C; D; EFlat; F; G; A; B ]
            createScale Dorianb2 C |> should equal [ C; DFlat; EFlat; F; G; A; BFlat ]
            createScale NeapolitanMinor C |> should equal [ C; DFlat; EFlat; F; G; AFlat; B ]
            createScale LydianAugmented C |> should equal [ C; D; E; FSharp; GSharp; A; B ]
            createScale LydianDominant C |> should equal [ C; D; E; FSharp; G; A; BFlat ]
            createScale Mixolydianb6 C |> should equal [ C; D; E; F; G; AFlat; BFlat ]
            createScale LocrianSharp2 C |> should equal [ C; D; EFlat; F; GFlat; AFlat; BFlat ]
            createScale AlteredDominant C |> should equal [ C; DFlat; DSharp; E; GFlat; GSharp; BFlat ]
            createScale HalfWholeDiminished C |> should equal [ C; DFlat; EFlat; E; FSharp; G; A; BFlat ]
            createScale WholeTone C |> should equal [ C; D; E; GFlat; GSharp; BFlat ]

        [<Property>]
        let ``It should return scales fitting a major triad`` (root :Note) =
            let chord = chord root ChordQuality.Major
            let chordNotes = chord.Notes |> List.map fst |> List.sort

            let scales = scalesFitting chord

            scales |> List.forall (
                fun s -> s.Notes |> List.filter (fun x -> (List.contains x chordNotes)) |> List.sort = chordNotes)

        [<Property>]
        let ``It should return scales fitting a minor triad`` (root :Note) =
            let chord = chord root ChordQuality.Minor
            let chordNotes = chord.Notes |> List.map fst |> List.sort

            let scales = scalesFitting chord

            scales |> List.forall (
                fun s -> s.Notes |> List.filter (fun x -> (List.contains x chordNotes)) |> List.sort = chordNotes)

        [<Property>]
        let ``It should return scales fitting a chord`` (root :Note) (quality: ChordQuality)=
            let chord = chord root quality
            let chordNotes = chord.Notes |> List.map fst |> List.sort

            let scales = scalesFitting chord

            scales |> List.forall (
                fun s -> s.Notes |> List.filter (fun x -> (List.contains x chordNotes)) |> List.sort = chordNotes)