Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports AlibreX

Namespace ADCP.Tools.Primitives

    ''' <summary>
    ''' WinForms dialog that collects primitive parameters and delegates creation to
    ''' <see cref="PrimitivesBuilder"/>.  The dialog stays open so multiple primitives
    ''' can be created in one session; the user closes it explicitly.
    ''' </summary>
    Friend NotInheritable Class PrimitivesDialog
        Inherits Form

        Private ReadOnly _part As IADPartSession

        Private WithEvents _comboPrimitive As ComboBox
        Private WithEvents _btnCreate As Button
        Private WithEvents _btnClose As Button

        Private _lblWidth As Label
        Private _numWidth As NumericUpDown
        Private _lblDepth As Label
        Private _numDepth As NumericUpDown
        Private _lblRadius As Label
        Private _numRadius As NumericUpDown
        Private _lblHeight As Label
        Private _numHeight As NumericUpDown

        Private Const _fieldW As Integer = 160
        Private Const _labelW As Integer = 120
        Private Const _rowH As Integer = 32
        Private Const _pad As Integer = 12

        ''' <summary>Initialises the dialog bound to <paramref name="part"/>, optionally preselecting a primitive.</summary>
        Public Sub New(part As IADPartSession, Optional preselect As String = Nothing)
            _part = part
            InitialiseComponent()
            If Not String.IsNullOrEmpty(preselect) Then
                Dim idx = _comboPrimitive.Items.IndexOf(preselect)
                If idx >= 0 Then _comboPrimitive.SelectedIndex = idx
            End If
            RefreshFields()
        End Sub

        Private Sub InitialiseComponent()
            SuspendLayout()

            Text = "Primitives"
            FormBorderStyle = FormBorderStyle.FixedDialog
            StartPosition = FormStartPosition.CenterScreen
            MaximizeBox = False
            MinimizeBox = False
            ShowIcon = False
            ShowInTaskbar = False
            TopMost = True
            BackColor = Color.FromArgb(250, 250, 250)
            ClientSize = New Size(_labelW + _fieldW + _pad * 3, _pad * 2 + _rowH * 6 + 8)
            AutoScaleMode = AutoScaleMode.Font

            Dim y As Integer = _pad

            _comboPrimitive = MakeCombo(y)
            For Each p As String In {"Box", "Cylinder", "Sphere", "Cone"}
                _comboPrimitive.Items.Add(p)
            Next
            _comboPrimitive.SelectedIndex = 0
            Controls.Add(MakeLabel("Primitive", y))
            Controls.Add(_comboPrimitive)
            y += _rowH + 4

            _lblWidth = MakeLabel("Width (mm)", y)
            _numWidth = MakeSpinner(y, 50, 0.01, 100000)
            Controls.Add(_lblWidth)
            Controls.Add(_numWidth)
            y += _rowH

            _lblDepth = MakeLabel("Depth (mm)", y)
            _numDepth = MakeSpinner(y, 50, 0.01, 100000)
            Controls.Add(_lblDepth)
            Controls.Add(_numDepth)
            y += _rowH

            _lblRadius = MakeLabel("Radius (mm)", y)
            _numRadius = MakeSpinner(y, 25, 0.01, 100000)
            Controls.Add(_lblRadius)
            Controls.Add(_numRadius)
            y += _rowH

            _lblHeight = MakeLabel("Height (mm)", y)
            _numHeight = MakeSpinner(y, 50, 0.01, 100000)
            Controls.Add(_lblHeight)
            Controls.Add(_numHeight)
            y += _rowH + 8

            _btnCreate = MakeButton("Create", _pad, y, primary:=True)
            _btnClose = MakeButton("Close", _pad + 130, y, primary:=False)
            Controls.Add(_btnCreate)
            Controls.Add(_btnClose)
            Height = y + _rowH + _pad + SystemInformation.CaptionHeight + 8

            ResumeLayout(performLayout:=True)
        End Sub

        Private Function MakeLabel(text As String, top As Integer) As Label
            Return New Label With {
                .Text = text,
                .Left = _pad,
                .Top = top + 6,
                .Width = _labelW,
                .AutoSize = False,
                .Font = New Font(SystemFonts.DefaultFont, FontStyle.Regular),
                .ForeColor = Color.FromArgb(64, 64, 64)
            }
        End Function

        Private Function MakeCombo(top As Integer) As ComboBox
            Return New ComboBox With {
                .Left = _pad + _labelW,
                .Top = top + 2,
                .Width = _fieldW,
                .Height = _rowH - 4,
                .DropDownStyle = ComboBoxStyle.DropDownList,
                .Font = SystemFonts.DefaultFont
            }
        End Function

        Private Function MakeSpinner(top As Integer, value As Decimal, minimum As Decimal, maximum As Decimal) As NumericUpDown
            Return New NumericUpDown With {
                .Left = _pad + _labelW,
                .Top = top + 2,
                .Width = _fieldW,
                .Height = _rowH - 4,
                .Minimum = minimum,
                .Maximum = maximum,
                .Value = value,
                .DecimalPlaces = 2,
                .Font = SystemFonts.DefaultFont,
                .BackColor = Color.White
            }
        End Function

        Private Function MakeButton(text As String, left As Integer, top As Integer, primary As Boolean) As Button
            Dim b As New Button With {
                .Text = text,
                .Left = left,
                .Top = top,
                .Width = 120,
                .Height = _rowH,
                .Font = SystemFonts.DefaultFont,
                .FlatStyle = FlatStyle.Flat,
                .Cursor = Cursors.Hand
            }
            If primary Then
                b.BackColor = Color.FromArgb(0, 122, 204)
                b.ForeColor = Color.White
                b.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204)
            Else
                b.BackColor = Color.FromArgb(240, 240, 240)
                b.ForeColor = Color.FromArgb(64, 64, 64)
                b.FlatAppearance.BorderColor = Color.FromArgb(204, 204, 204)
            End If
            b.FlatAppearance.BorderSize = 1
            Return b
        End Function

        Private Sub RefreshFields()
            If _numWidth Is Nothing OrElse _numDepth Is Nothing OrElse
               _numRadius Is Nothing OrElse _numHeight Is Nothing Then
                Return
            End If

            Dim sel As String = If(_comboPrimitive.SelectedItem?.ToString(), "Box")
            _lblWidth.Visible = (sel = "Box")
            _numWidth.Visible = (sel = "Box")
            _lblDepth.Visible = (sel = "Box")
            _numDepth.Visible = (sel = "Box")
            _lblRadius.Visible = (sel = "Cylinder" OrElse sel = "Sphere" OrElse sel = "Cone")
            _numRadius.Visible = (sel = "Cylinder" OrElse sel = "Sphere" OrElse sel = "Cone")
            _lblHeight.Visible = (sel = "Box" OrElse sel = "Cylinder" OrElse sel = "Cone")
            _numHeight.Visible = (sel = "Box" OrElse sel = "Cylinder" OrElse sel = "Cone")
        End Sub

        Private Sub OnPrimitiveChanged(sender As Object, e As EventArgs) Handles _comboPrimitive.SelectedIndexChanged
            RefreshFields()
        End Sub

        Private Sub OnCreate(sender As Object, e As EventArgs) Handles _btnCreate.Click
            Dim sel As String = If(_comboPrimitive.SelectedItem?.ToString(), "Box")
            Try
                Select Case sel
                    Case "Box"
                        PrimitivesBuilder.CreateBox(
                            _part,
                            CDbl(_numWidth.Value),
                            CDbl(_numDepth.Value),
                            CDbl(_numHeight.Value))
                    Case "Cylinder"
                        PrimitivesBuilder.CreateCylinder(
                            _part,
                            CDbl(_numRadius.Value),
                            CDbl(_numHeight.Value))
                    Case "Sphere"
                        PrimitivesBuilder.CreateSphere(
                            _part,
                            CDbl(_numRadius.Value))
                    Case "Cone"
                        PrimitivesBuilder.CreateCone(
                            _part,
                            CDbl(_numRadius.Value),
                            CDbl(_numHeight.Value))
                End Select
            Catch ex As Exception
                MessageBox.Show(
                    $"Failed to create {sel}:{Environment.NewLine}{ex.Message}",
                    "Primitives",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub OnClose(sender As Object, e As EventArgs) Handles _btnClose.Click
            Close()
        End Sub

    End Class

End Namespace
