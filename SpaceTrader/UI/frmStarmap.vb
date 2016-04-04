Imports System.Drawing

Public Class frmStarmap
    Friend starmap As starmap = starmap.build(58459025)

    Private Sub frmStarmap_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        displayStarmap()
    End Sub
    Private Sub star_Click(ByVal sender As Windows.Forms.Panel, ByVal e As System.EventArgs)
        Dim star As star = CType(sender.Tag, star)
        displayStar(star)
    End Sub
    Private Sub planet_Click(ByVal sender As Windows.Forms.Panel, ByVal e As System.EventArgs)
        Dim planet As planet = CType(sender.Tag, planet)
        displayPlanet(planet)
    End Sub

    Private tabStarControls As New List(Of Windows.Forms.Control)
    Private tabPlanetControls As New List(Of Windows.Forms.Control)
    Private Sub displayStarmap()
        For Each star In starmap.stars
            Dim img As New System.Windows.Forms.Panel
            With img
                .Location = New Point(star.xy.x, star.xy.y)
                .Size = New Size(star.xySize, star.xySize)
                .BackgroundImage = Image.FromFile("img/stars/" & star.imgString & ".png", True)
                .BackgroundImageLayout = Windows.Forms.ImageLayout.Stretch
                .Visible = True
                .Tag = star
            End With
            tt.SetToolTip(img, star.tooltipReport)
            tabGalaxy.Controls.Add(img)
            AddHandler img.Click, AddressOf star_Click
        Next
    End Sub
    Private Sub displayStar(ByRef star As star)
        For Each panel In tabStarControls
            tabStar.Controls.Remove(panel)
        Next
        tabStarControls.Clear()
        tabStar.BackgroundImage = Image.FromFile("img/starviews/" & star.imgString & ".jpg", True)

        For Each planet In star.planets
            Dim img As New System.Windows.Forms.Panel
            With img
                .Location = New Point(planet.xy.x, planet.xy.y)
                .Size = New Size(planet.xySize, planet.xySize)
                .BackgroundImage = Image.FromFile("img/planets/" & planet.imgString & ".png", True)
                .BackgroundImageLayout = Windows.Forms.ImageLayout.Stretch
                .Visible = True
                .Tag = planet
            End With
            tt.SetToolTip(img, planet.tooltipReport)
            tabStar.Controls.Add(img)
            tabStarControls.Add(img)
            AddHandler img.Click, AddressOf planet_Click
        Next

        tabStar.Invalidate()
        TabControl1.SelectedTab = tabStar
    End Sub
    Private Sub displayPlanet(ByRef planet As planet)


    End Sub
End Class