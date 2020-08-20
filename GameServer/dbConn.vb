Imports System.Data.SqlClient
Public Class dbConn
    Public Property strSQL As String
    Private objConn As SqlClient.SqlConnection =
            New SqlClient.SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alem2\Downloads\Dropbox\GameServer\GameServer\GameServer\GameServer\Database1.mdf;Integrated Security=True")
    Public Function getDt() As DataTable
        Dim Conn As New SqlClient.SqlConnection
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Try
            objConn.Open()
            da = New SqlClient.SqlDataAdapter(strSQL, objConn)
            da.Fill(dt)
            da = Nothing
            objConn.Close()
            objConn = Nothing
        Catch ex As Exception
            objConn.Close()
            objConn = Nothing
        End Try
        Return dt
    End Function
End Class