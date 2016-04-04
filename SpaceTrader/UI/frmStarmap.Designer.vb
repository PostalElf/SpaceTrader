<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStarmap
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStarmap))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabGalaxy = New System.Windows.Forms.TabPage()
        Me.tabStar = New System.Windows.Forms.TabPage()
        Me.tabPlanet = New System.Windows.Forms.TabPage()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Left
        Me.TabControl1.Controls.Add(Me.tabGalaxy)
        Me.TabControl1.Controls.Add(Me.tabStar)
        Me.TabControl1.Controls.Add(Me.tabPlanet)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(700, 500)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl1.TabIndex = 0
        Me.TabControl1.TabStop = False
        '
        'tabGalaxy
        '
        Me.tabGalaxy.BackColor = System.Drawing.Color.Black
        Me.tabGalaxy.BackgroundImage = CType(resources.GetObject("tabGalaxy.BackgroundImage"), System.Drawing.Image)
        Me.tabGalaxy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.tabGalaxy.Location = New System.Drawing.Point(23, 4)
        Me.tabGalaxy.Name = "tabGalaxy"
        Me.tabGalaxy.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGalaxy.Size = New System.Drawing.Size(673, 492)
        Me.tabGalaxy.TabIndex = 0
        Me.tabGalaxy.Text = "Galaxy"
        '
        'tabStar
        '
        Me.tabStar.BackColor = System.Drawing.Color.Black
        Me.tabStar.BackgroundImage = CType(resources.GetObject("tabStar.BackgroundImage"), System.Drawing.Image)
        Me.tabStar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.tabStar.Location = New System.Drawing.Point(23, 4)
        Me.tabStar.Name = "tabStar"
        Me.tabStar.Padding = New System.Windows.Forms.Padding(3)
        Me.tabStar.Size = New System.Drawing.Size(673, 492)
        Me.tabStar.TabIndex = 1
        Me.tabStar.Text = "Star"
        '
        'tabPlanet
        '
        Me.tabPlanet.BackColor = System.Drawing.Color.Black
        Me.tabPlanet.BackgroundImage = CType(resources.GetObject("tabPlanet.BackgroundImage"), System.Drawing.Image)
        Me.tabPlanet.Location = New System.Drawing.Point(23, 4)
        Me.tabPlanet.Name = "tabPlanet"
        Me.tabPlanet.Size = New System.Drawing.Size(673, 492)
        Me.tabPlanet.TabIndex = 2
        Me.tabPlanet.Text = "Planet"
        '
        'tt
        '
        Me.tt.AutomaticDelay = 0
        Me.tt.UseAnimation = False
        Me.tt.UseFading = False
        '
        'frmStarmap
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(736, 535)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmStarmap"
        Me.Text = "Starmap"
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabGalaxy As System.Windows.Forms.TabPage
    Friend WithEvents tabStar As System.Windows.Forms.TabPage
    Friend WithEvents tabPlanet As System.Windows.Forms.TabPage
    Friend WithEvents tt As System.Windows.Forms.ToolTip
End Class
