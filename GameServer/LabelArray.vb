Public Class LabelArray
    Inherits System.Collections.CollectionBase
    Private ReadOnly HostForm As  _
 System.Windows.Forms.Form
    Public Function AddNewLabel() _
 As System.Windows.Forms.Label
        ' Create a new instance of the Label class.
        Dim aLabel As New System.Windows.Forms.Label
        ' Add the Label to the collection's
        ' internal list.
        Me.List.Add(aLabel)
        ' Add the Label to the Controls collection   
        ' of the Form referenced by the HostForm field.
        HostForm.Controls.Add(aLabel)
        ' Set intial properties for the Label object.
        aLabel.Top = Count * 25
        aLabel.Width = 50
        aLabel.Left = 140
        aLabel.Tag = Me.Count
        aLabel.Text = "Label " & Me.Count.ToString
        Return aLabel
    End Function
    Public Sub New( _
 ByVal host As System.Windows.Forms.Form)
        HostForm = host
        Me.AddNewLabel()
    End Sub
    Default Public ReadOnly Property _
        Item(ByVal Index As Integer) As  _
        System.Windows.Forms.Label
        Get
            Return CType(Me.List.Item(Index),  _
  System.Windows.Forms.Label)
        End Get
    End Property
    Public Sub Remove()
        ' Check to be sure there is a Label to remove.
        If Me.Count > 0 Then
            ' Remove the last Label added to the array 
            ' from the host form controls collection. 
            ' Note the use of the default property in 
            ' accessing the array.
            HostForm.Controls.Remove(Me(Me.Count - 1))
            Me.List.RemoveAt(Me.Count - 1)
        End If
    End Sub
End Class
