Imports System.IO
Imports Microsoft.VisualBasic.FileIO

<DebuggerStepThrough()> Module valve
    Public rng As New Random

    Public Function vbSpace(Optional ByVal times As Integer = 1) As String
        If times = 0 Then Return Nothing

        Dim total As String = Nothing
        For n = 1 To times
            total &= "   "
        Next
        Return total
    End Function
    Public Function fakeTab(ByVal word As String, ByVal totalLength As Integer)
        Dim spaceCount As Integer = totalLength - word.Length
        Dim spaces As String = ""
        For n = 1 To spaceCount
            spaces &= " "
        Next
        Return word & spaces
    End Function

    Public Function constrain(ByVal value As Integer, Optional ByVal minValue As Integer = 1, Optional ByVal maxValue As Integer = 100) As Integer
        Dim total As Integer = value
        If total < minValue Then total = minValue
        If total > maxValue Then total = maxValue
        Return total
    End Function
    Public Function constrain(ByVal value As Integer, ByVal range As range) As Integer
        Return constrain(value, range.min, range.max)
    End Function
    Public Function circular(ByVal value As Integer, Optional ByVal minValue As Integer = 1, Optional ByVal maxValue As Integer = 4) As Integer
        Dim total As Integer = value
        While total < minValue OrElse total > maxValue
            If total < minValue Then total += maxValue
            If total > maxValue Then total -= maxValue
        End While
        Return total
    End Function

    Private Function sign(ByVal value As Decimal) As String
        If value < 0 Then Return "-" Else Return "+"
    End Function
    Public Function withSign(ByVal value As Decimal, Optional ByVal prefix As String = "") As String
        Dim valueStr As String
        Select Case prefix
            Case "$" : valueStr = Math.Abs(value).ToString("N0")
            Case Else : valueStr = Math.Abs(value)
        End Select

        Return sign(value) & prefix & valueStr
    End Function
    Public Function withReverseSign(ByVal value As Decimal, Optional ByVal prefix As String = "") As String
        Return withSign(value * -1, prefix)
    End Function
    Public Function withCommas(ByVal inputList As List(Of String)) As String
        Dim total As String = ""
        For n = 0 To inputList.Count - 1
            total &= inputList(n)
            If n < inputList.Count - 1 Then total &= ", "
        Next
        Return total
    End Function
    Public Function nounChildGendered(ByVal gender As eGender) As String
        Select Case gender
            Case eGender.Male : Return "boy"
            Case eGender.Female : Return "girl"
            Case eGender.Thing : Return "thing"
            Case Else : Return Nothing
        End Select
    End Function
    Public Function nounAdultGendered(ByVal gender As eGender) As String
        Select Case gender
            Case eGender.Male : Return "man"
            Case eGender.Female : Return "woman"
            Case eGender.Thing : Return "thing"
            Case Else : Return Nothing
        End Select
    End Function
    Public Function pronounGendered(ByVal gender As eGender) As String
        Select Case gender
            Case eGender.Male : Return "him"
            Case eGender.Female : Return "her"
            Case eGender.Thing : Return "it"
            Case Else : Return Nothing
        End Select
    End Function
    Public Function romanNumeral(ByVal n As Integer) As String
        If n = 0 Then Return 0
        ' there is no Roman symbol for 0, but we don't want to return an empty string

        Const r = "IVXLCDM" ' Roman symbols
        Dim i As Integer = Math.Abs(n)
        Dim s As String = ""

        For p As Integer = 1 To 5 Step 2
            Dim d As Integer = i Mod 10
            i = i \ 10
            Select Case d ' format a decimal digit
                Case 0 To 3 : s = s.PadLeft(d + Len(s), Mid(r, p, 1))
                Case 4 : s = Mid(r, p, 2) & s
                Case 5 To 8 : s = Mid(r, p + 1, 1) & s.PadLeft(d - 5 + Len(s), Mid(r, p, 1))
                Case 9 : s = Mid(r, p, 1) & Mid(r, p + 2, 1) & s
            End Select
        Next

        s = s.PadLeft(i + Len(s), "M") ' format thousands
        If n < 0 Then s = "-" & s ' insert sign if negative (non-standard)
        Return s
    End Function

    Public Function percentRoll(ByVal probability As Integer, Optional ByRef r As Random = Nothing) As Boolean
        If r Is Nothing Then r = rng
        Dim roll As Integer = r.Next(1, 101)
        If roll <= probability Then Return True Else Return False
    End Function
    Public Function lumpyRng(ByVal min As Integer, ByVal max As Integer, Optional ByRef r As Random = Nothing)
        'min is inclusive while max is exclusive
        If r Is Nothing Then r = rng
        Dim total As Integer = 0
        For n = 1 To 3
            total += r.Next(min, max)
        Next
        Return Int(total / 3)
    End Function
    Public Function rollDice(ByVal dice As String) As Integer
        '3d6
        Dim diceText As String() = dice.Split("d")
        If IsNumeric(diceText(0)) = False OrElse IsNumeric(diceText(1)) = False Then Return 0

        Dim numOfDice As Integer = CInt(diceText(0))
        Dim diceFace As Integer = CInt(diceText(1))
        Dim total As Integer = 0
        For n = 1 To numOfDice
            total += rng.Next(1, diceFace + 1)
        Next
        Return total
    End Function
    Public Function cascadingRng(ByVal value As Integer, ByVal min As Integer, ByVal max As Integer) As Integer
        'returns probability value (0-100) that's lower the closer it gets to min

        Dim range As Integer = max - min
        Return Int(range / 100 * value)
    End Function
    Public Function coinFlip(Optional ByRef r As Random = Nothing) As Boolean
        If r Is Nothing Then r = rng
        If r.Next(0, 2) = 1 Then Return True Else Return False
    End Function
    Public Function pythogoras(ByVal xy1 As xy, ByVal xy2 As xy) As Integer
        Dim x As Integer = Math.Abs(xy1.x - xy2.x)
        Dim y As Integer = Math.Abs(xy1.y - xy2.y)
        Return Math.Round(Math.Sqrt(x * x + y * y))
    End Function

    Public Function getLiteral(ByVal value As Integer, Optional ByVal sigDigits As Integer = 2) As String
        Dim str As String = ""
        For n = 1 To sigDigits
            str = str & "0"
        Next
        str = str & value

        Dim characters() As Char = StrReverse(str)
        str = Nothing
        For n = 0 To sigDigits - 1
            str = str & characters(n)
        Next

        Return StrReverse(str)
    End Function
    Public Function joinString(ByVal stringQueue As Queue(Of String)) As String
        Dim total As String = ""
        While stringQueue.Count > 0
            total &= stringQueue.Dequeue
        End While
        Return total
    End Function
    Public Function writeDash(ByVal n As Integer) As String
        Dim str As String = ""
        For count = 0 To n
            str &= ("-")
        Next
        Return str
    End Function
    Public Function stripS(ByVal str As String) As String
        If str(str.Count - 1) = "s" Then
            Dim split As String = str.TrimEnd("s")
            Return split
        Else
            Return str
        End If
    End Function
    Public Function aOrAn(ByVal nextWord As String) As String
        Dim vowels As String = "aeiou"
        For Each c As Char In vowels
            If nextWord.ToLower.StartsWith(c) Then Return "an " & nextWord
        Next
        Return "a " & nextWord
    End Function

    Public Function fileget(ByVal pathname As String) As List(Of String)
        Dim templist As New List(Of String)

        Using sr As New StreamReader(pathname)
            Do While sr.Peek <> -1
                Dim line As String = sr.ReadLine
                templist.Add(line)
            Loop
        End Using

        Return templist
    End Function
    Public Function bracketFileget(ByVal pathname As String, ByVal bracketString As String) As Queue(Of String)
        Dim total As New Queue(Of String)
        Try
            Dim line As String
            Using sr As New StreamReader(pathname)
                While sr.Peek <> -1
                    line = sr.ReadLine
                    If line = "[" & bracketString & "]" Then
                        'found bracketString
                        total.Enqueue(bracketString)

                        'keep reading until next bracket
                        While sr.Peek <> -1
                            line = sr.ReadLine
                            If line.StartsWith("[") Then
                                'reached next bracketed section, stop searching
                                Return total
                            Else
                                'make sure that line doesn't start with comment char (-) and that line is not empty
                                If line <> "" AndAlso line.StartsWith("-") = False Then total.Enqueue(line)
                            End If
                        End While
                    End If
                End While
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return Nothing
        End Try
        Return total
    End Function
    Public Function bracketFilegetAll(ByVal pathname As String) As List(Of Queue(Of String))
        Dim total As New List(Of Queue(Of String))
        Try
            Dim line As String
            Using sr As New StreamReader(pathname)
                Dim current As New Queue(Of String)
                While sr.Peek <> -1
                    line = sr.ReadLine
                    If line.StartsWith("-") Then Continue While
                    If line.StartsWith("[") Then
                        'remove brackets
                        line = line.Remove(0, 1)
                        line = line.Remove(line.Length - 1, 1)

                        'if current is filled, add to total
                        If current.Count > 0 Then total.Add(current)

                        'start new current with bracketstring as header
                        current = New Queue(Of String)
                        current.Enqueue(line)
                    Else
                        If line <> "" Then current.Enqueue(line)
                    End If
                End While

                'add last entry
                If current.Count > 0 Then total.Add(current)
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return Nothing
        End Try
        Return total
    End Function
    Public Function csvFileget(ByVal pathname As String) As List(Of String())
        Dim total As New List(Of String())

        Using parser As New TextFieldParser(pathname)
            parser.SetDelimiters(",")
            parser.HasFieldsEnclosedInQuotes = True

            'skip header
            parser.ReadLine()

            While parser.EndOfData = False
                Dim currentLine As String() = parser.ReadFields
                If currentLine(0) <> "" Then total.Add(currentLine)
            End While
        End Using

        Return total
    End Function

    Public Function getRandomAndRemove(ByRef objList As List(Of String), ByVal pathname As String, Optional ByRef r As Random = Nothing) As Object
        If r Is Nothing Then r = rng
        If objList.Count = 0 Then objList = fileget(pathname)
        Dim c As Integer = r.Next(objList.Count)
        getRandomAndRemove = objList(c)
        objList.RemoveAt(c)
    End Function
End Module


<DebuggerStepThrough()> Public Structure range
    Public Property min As Integer
    Public Property max As Integer

    Public Sub New(ByVal _min As Integer, ByVal _max As Integer)
        min = _min
        max = _max
    End Sub
    Public Overrides Function ToString() As String
        If min = max Then Return min Else Return min & "-" & max
    End Function

    Public Function roll() As Integer
        Return rng.Next(min, max + 1)
    End Function
    Public Function isWithin(ByVal value As Integer) As Boolean
        If value >= min AndAlso value <= max Then Return True Else Return False
    End Function
End Structure

<DebuggerStepThrough()> Public Structure xy
    Public Property x As Integer
    Public Property y As Integer

    Public Sub New(ByVal aX As Integer, ByVal aY As Integer)
        x = aX
        y = aY
    End Sub
    Public Overrides Function ToString() As String
        Return x & "," & y
    End Function
End Structure

Public Enum eGender
    Male = 1
    Female
    Thing
End Enum

<DebuggerStepThrough()> Public Class rollingCounter
    Private Property value As Integer

    Public Sub New(ByVal aValue As Integer)
        value = aValue
    End Sub
    Public Function Tick() As Integer
        Tick = value
        value += 1
    End Function
    Public Function Last() As Integer
        Return value - 1
    End Function
End Class