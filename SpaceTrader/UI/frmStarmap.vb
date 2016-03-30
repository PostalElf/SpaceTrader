Imports System.Drawing

Public Class frmStarmap
    Friend starmap As starmap = starmap.build(58459025)


    Private Sub displayStarmap()
        Debug.Print(vbCrLf)
        For Each star In starmap.stars
            Dim realX As Integer = star.xy.x / star.maxX * Me.Width
            Dim realY As Integer = star.xy.y / star.maxY * Me.Height
            Debug.Print(star.name & ": " & realX & "/" & Me.Width & "," & realY & "/" & Me.Height)
            Dim img As New System.Windows.Forms.PictureBox
            With img
                .Location = New Point(realX, realY)
                .Size = New Size(25, 25)
                .Image = Image.FromFile("img/fireflake.png")
                .Visible = True
            End With
            Me.Controls.Add(img)
        Next
    End Sub

    Private Sub frmStarmap_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        displayStarmap()
    End Sub
End Class