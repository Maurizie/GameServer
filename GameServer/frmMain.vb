Imports System.IO
Imports System.Data.SqlClient
Public Class frmMain

    Dim WithEvents _Server As NetComm.Host

    Dim Lookup As String() = {"login", "auth", "register", "tutorial", "resources", "getallstructures", "structures", "research"}
    Dim usernames As String()
    Dim playerlist As New Dictionary(Of String, Player)
    Dim currentplayer As Player
    Public StructureList As New Dictionary(Of String, Structures)
    Public ResearchList As New Dictionary(Of String, Research)


    Private Function AddLog(ByVal Text As String)
        txtLogs.AppendText("[" & Now.ToShortTimeString & "] " & Text & vbNewLine)
        Return Nothing
    End Function

    Public Function SendMessage(ByVal Username As String, ByVal LookupID As String, Optional ByVal Message As String = vbNullString)
        _Server.SendData(Username, NetComm.Host.Convert2Ascii(Array.IndexOf(Lookup, LookupID).ToString("00") & Message))
        Return Nothing
    End Function

    Private Sub DataReceived(ByVal ID As String, ByVal Data() As Byte) Handles _Server.DataReceived
        Dim SData As String = NetComm.Host.ConvertFromAscii(Data)
        AddLog(ID & ": " & SData)
        Dim LookupID As Integer = Convert.ToInt32(SData.Substring(0, 2))

        Select Case Lookup(LookupID)
            Case "login"
                Dim PasswordHash As String = SData.Substring(2, SData.Length - 2)
                ' Dim path As String = "C:\ProjGame\" & ID.ToLower & "\basics.txt"
                'Check password
                Dim Loginsql As String = "select top 1 Password from [Player] where [User] = '" & ID.ToLower & "'"
                If SQLSingleValue(Loginsql) <> "null" Then

                    If PasswordHash = SQLSingleValue(Loginsql) Then

                        AddLog(ID & " authenticated")
                        Dim temp As String = ""
                        temp = CheckTutorialComplete(ID)
                        Select Case temp
                            Case "True"
                                SendMessage(ID, "tutorial", "1")
                            Case "False"
                                SendMessage(ID, "tutorial", "0")
                                If Not playerlist.ContainsKey(ID) Then
                                    playerlist.Add(ID, New Player(ID))
                                End If
                                SendMessage(ID, "login", "1")
                            Case "ERROR"
                                SendMessage(ID, "tutorial", "7")
                        End Select
                    Else
                        SendMessage(ID, "login", "0")
                        AddLog("Incorrect password attempt: " & ID)
                        _Server.DisconnectUser(ID)
                    End If
                Else
                    SendMessage(ID, "login", "0")
                    AddLog("No user registered: " & ID)
                    _Server.DisconnectUser(ID)
                End If

            Case "register"
                Dim PasswordHash As String = SData.Substring(2, SData.Length - 2)
                If Register(ID, PasswordHash) Then
                    SendMessage(ID, "register", "1")
                Else
                    SendMessage(ID, "register", "0")
                End If
            Case "tutorial"
                Dim message = SData.Substring(2, SData.Length - 2)
                If message = "0" Then
                    message = "spazzes"
                    CompleteTutorial(message, ID)
                Else
                    message = "tauri"
                    CompleteTutorial(message, ID)
                End If

            Case "resources"
                Dim message As String = SData.Substring(2, SData.Length - 2)
                If message = "getresources" Then
                    SendMessage(ID, "resources", playerlist(ID).EncodeResources())
                End If
            Case "getallstructures"
                Dim message As String = SData.Substring(2, SData.Length - 2)
                SendMessage(ID, "getallstructures", playerlist(ID).EncodeStructureLevels())
            Case "structures"
                Dim message As String = SData.Substring(2, SData.Length - 2)
                playerlist(ID).UpgradeBuilding(message)

                SendMessage(ID, "getallstructures", playerlist(ID).EncodeStructureLevels())
                SendMessage(ID, "resources", playerlist(ID).EncodeResources())
            Case "research"
                Dim message As String = SData.Substring(2, SData.Length - 2)
                If Mid(message, 1, 3) = "get" Then
                    playerlist(ID).RequestResearchLevels(message)
                End If
                If Mid(message, 1, 5) = "check" Then
                    playerlist(ID).CheckResearchCost(message)
                End If
                If Mid(message, 1, 2) = "up" Then
                    playerlist(ID).ResearchTechnology(Mid(message, 3))
                End If
        End Select
    End Sub

    Private Sub DataTransferred(ByVal Sender As String, ByVal Recipient As String, ByVal Data() As Byte) Handles _Server.DataTransferred
        AddLog(Sender & " sent data to " & Recipient & ": " & NetComm.Host.ConvertFromAscii(Data))
    End Sub

    Public Sub onConnection(ByVal ID As String) Handles _Server.onConnection
        AddLog(ID & " connected")
        lbPlayers.Items.Add(ID)
        SendMessage(ID, "auth")
    End Sub

    Private Sub lostConnection(ByVal ID As String) Handles _Server.lostConnection
        AddLog(ID & " disconnected")
        If playerlist.ContainsKey(ID) Then
            playerlist(ID).CalculateMaterials()
            playerlist.Remove(ID)
        End If
    End Sub

    Private Sub errHandler(ByVal ex As Exception) Handles _Server.errEncounter
        If ex.Message <> "A blocking operation was interrupted by a call to WSACancelBlockingCall" Then
            txtLogs.AppendText(ex.Message)
        End If
    End Sub

    Private Sub txtCommand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCommand.KeyDown
        If e.KeyCode() = 13 Then
            cmdEnter_Click(sender, New System.EventArgs)
        End If
    End Sub


    Private Sub cmdEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnter.Click
        If Me.txtCommand.Text.Trim.Length > 0 Then
            _Server.Brodcast(NetComm.Host.Convert2Ascii(txtCommand.Text.Trim))
        End If
    End Sub

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        '_Server.Start(777)
        _Server = New NetComm.Host(777)
        _Server.StartConnection()
        cmdStart.Enabled = False
        cmdStop.Enabled = True
        AddLog("Starting server")
        SetUpStructures()
        SetUpResearchRequirements()
    End Sub

    Private Sub cmdStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        Dim result As MsgBoxResult = MsgBox("Are you sure you want to stop the server?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Stop?")
        If result = MsgBoxResult.Yes Then
            _Server.CloseConnection()
            SaveCurrentDate()
            cmdStart.Enabled = True
            cmdStop.Enabled = False
            AddLog("Stopping server")
        End If
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveCurrentDate()
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lbPlayers.Items.Clear()
        Dim test As New SqlClient.SqlCommand

    End Sub

    Private Function Register(ByVal Username As String, ByVal Password As String)
        Dim Items() As String = {Password}
        Dim filepath As String = "C:\ProjGame" & "\" & Username.ToLower & "\basics.txt"
        Dim filepathcreate As String = "C:\ProjGame\" & Username.ToLower
        'Dim dt As New DataTable
        'Dim da As New dbConn
        'da.strSQL = "select top 1 [User] from [Player] where [User] = '" & Username.ToLower & "'"
        'dt = da.getDt
        Dim sql As String = "select top 1 [User] from [Player] where [User] = '" & Username.ToLower & "'"
        If SQLSingleValue(sql) <> "null" Then
            Return False
        End If
        Dim query As String = String.Empty
        query &= "INSERT INTO Player ([User], Password, Tutorial, Metal, Silicon, Tritium, Login, MetalFoundry, SiliconExtractor, TritiumGenerato, ResearchLab, RoboticsFactory, Shipyard, WeaponsTech, DefenceTech, ShieldingTech, EspionageTech, RocketEngineTech, SolarEngineTech, NuclearFusionTech, ColonisationTech, HyperspaceEngineTech, WarpDriveTech,EnergyTech) "
        query &= "VALUES ('" & Username.ToLower & "', '" & Items(0) & "', '1', '40', '20', '0', '" & CStr(System.DateTime.Now) & "', '1', '1', '1', '1', '1', '1','0','0','0','0','0','0','0','0','0','0','0')"
        InsertSQL(query)
        'System.IO.Directory.CreateDirectory(filepathcreate)

        'Dim SW As New StreamWriter(filepath, True)


        'For Each Item As String In Items
        '    SW.WriteLine(" -- BASICS -- ")
        ' SW.WriteLine(Item)
        '    SW.WriteLine("True")
        '    SW.WriteLine("")
        '    SW.WriteLine(" -- RESOURCES -- ")
        '    SW.WriteLine("m 40")
        '    SW.WriteLine("s 20")
        '    SW.WriteLine("t 0")
        '    SW.WriteLine(" -- LAST LOGIN -- ")
        '    SW.WriteLine(CStr(System.DateTime.Now))
        '    SW.WriteLine(" -- STRUCTURES -- ")
        '    SW.WriteLine("mf 1") ' Metal Foundary
        '    SW.WriteLine("se 1") ' Silicon Extractor
        '    SW.WriteLine("tg 1") ' Tritium Generator
        '    SW.WriteLine("rl 1") ' Research Lab
        '    SW.WriteLine("sp 1") ' Solar Plant
        '    SW.WriteLine("sy 1") ' Shipyard 
        '    SW.WriteLine(" -- RESEARCH -- ") '
        '    SW.WriteLine("wt 0")
        '    SW.WriteLine("dt 0")
        '    SW.WriteLine("st 0")
        '    SW.WriteLine("fe 0")
        '    SW.WriteLine("re 0")
        '    SW.WriteLine("se 0")
        '    SW.WriteLine("nf 0")
        '    SW.WriteLine("ct 0")
        '    SW.WriteLine("he 0")
        '    SW.WriteLine("wd 0")
        'Next

        'SW.Close()
        'playerlist.Add(Username, New Player(Username))
        Return True
    End Function
    Function CheckTutorialComplete(ByVal username As String) As String
        Dim strreturn As String
        '  Dim filepath As String = "C:\ProjGame" & "\" & username.ToLower & "\basics.txt"
        ' If Not File.Exists(filepath) Then
        'Return "ERROR"
        'E lse
        'Dim textfile() As String = File.ReadAllLines(filepath)
        'strreturn = textfile(2)
        Dim SQLTutCheck = "Select Tutorial From Player Where [USER] = '" & username.ToLower & "'"
        strreturn = SQLSingleValue(SQLTutCheck)
        playerlist.Add(username, New Player(username))
        Return strreturn


    End Function
    Sub CompleteTutorial(ByVal faction As String, ByVal username As String)
        'Dim filepath As String = "C:\ProjGame" & "\" & username.ToLower & "\basics.txt"
        ' Dim filedata() As String = File.ReadAllLines(filepath)
        'filedata(2) = "False"
        Dim value As Boolean
        If faction = "spazzes" Then
            '  filedata(3) = "0"
            value = False
        Else
            ' filedata(3) = "1"
            value = True
        End If
        Dim sqlTutfin As String = "update player set faction = '" & value & "', Tutorial = 'False' where [User] = '" & username.ToLower & "'"
        InsertSQL(sqlTutfin)
        'File.WriteAllLines(filepath, filedata)

    End Sub
    Sub GetResource(ByVal ID As String)
        currentplayer = playerlist(ID)
        SendMessage(ID, "resources", "m" & currentplayer.GetMetal)
        SendMessage(ID, "resources", "s" & currentplayer.GetSIlicon)
        SendMessage(ID, "resources", "t" & currentplayer.GetTritium)
        SavePlayerLoginDate(ID)
    End Sub

    Sub SaveCurrentDate()
        Dim filepath As String = "C:\ProjGame" & "\serverclose.txt"
        Dim SW As New StreamWriter(filepath, False)
        SW.WriteLine(CStr(DateTime.Now))
        SW.Close()
    End Sub
    Sub SavePlayerLoginDate(ByVal ID As String)
        playerlist(ID).ChangeLoginData()
    End Sub


    Function HandleTempError(ByVal filedata() As String, ByVal i As Integer) As String
        If filedata(i).Length > 2 Then
            Return filedata(i)
        Else : Return "   "
        End If
    End Function

    Sub GetALlStructures(ByVal ID As String)
        SendMessage(ID, "getallstructures", "mf" & playerlist(ID).GetMFLevel)
        SendMessage(ID, "getallstructures", "se" & playerlist(ID).GetSELevel)
        SendMessage(ID, "getallstructures", "tg" & playerlist(ID).GetTGLevel)
        SendMessage(ID, "getallstructures", "rl" & playerlist(ID).GetRLLevel)
        SendMessage(ID, "getallstructures", "rf" & playerlist(ID).GetRFLevel)
        SendMessage(ID, "getallstructures", "sy" & playerlist(ID).GetSYLevel)

    End Sub

    Public Sub SetUpResearchRequirements()
        Dim WeaponsTechnologyRequirements, EnergyTechnologyRequirements, DefenceTechnologyRequirements, ShieldTechnologyRequirements, SolarEngineRequirements, RocketEngineRequirements, HyperspaceEngineRequirements As New Dictionary(Of String, Integer)
        Dim WarpDriveTechnologyRequirements, ColonisationRequirements, EspionageTechnologyRequirements, NuclearFusionRequirements, HyperspaceTechRequirements As New Dictionary(Of String, Integer)


        Dim WeaponsTechnology As New Research(200, 100, 0, "WeaponsTech", WeaponsTechnologyRequirements, 2)
        Dim DefenceTechnology As New Research(400, 0, 0, "DefenceTech", DefenceTechnologyRequirements, 2)
        Dim ShieldTechnology As New Research(100, 200, 0, "ShieldingTech", ShieldTechnologyRequirements, 2)
        Dim EspionageTechnology As New Research(100, 200, 100, "EspionageTech", EspionageTechnologyRequirements, 1)
        Dim EnergyTechology As New Research(0, 800, 400, "EnergyTech", EnergyTechnologyRequirements, 1)

        SolarEngineRequirements.Add("EnergyTech", 2)
        RocketEngineRequirements.Add("EnergyTech", 3)
        HyperspaceTechRequirements.Add("ShieldingTech", 5) : HyperspaceEngineRequirements.Add("EnergyTech", 5)

        Dim RocketEngineTechnology As New Research(500, 300, 200, "RocketEngineTech", RocketEngineRequirements, 2)
        Dim SolarEngineTechnology As New Research(1000, 2000, 1000, "SolarEngineTech", SolarEngineRequirements, 3)
        Dim NuclearFusion As New Research(20000, 40000, 20000, "NuclearFusionTech", NuclearFusionRequirements, 6)
        Dim HyperspaceTech As New Research(0, 4000, 2000, "HyperspaceTech", HyperspaceTechRequirements, 7)

        ColonisationRequirements.Add("RocketEngineTech", 3) : ColonisationRequirements.Add("EspionageTech", 4)
        HyperspaceEngineRequirements.Add("EnergyTech", 5) : HyperspaceEngineRequirements.Add("ShieldingTech", 5) : HyperspaceEngineRequirements.Add("HyperspaceTech", 3)


        Dim ColonisationTechnology As New Research(10000, 20000, 10000, "ColonisationTech", ColonisationRequirements, 5)
        Dim HyperspaceEngineTechnology As New Research(60000, 120000, 40000, "HyperspaceEngineTech", HyperspaceEngineRequirements, 7)

        WarpDriveTechnologyRequirements.Add(HyperspaceEngineTechnology)

        Dim WarpDriveTechnology As New Research(1000000, 1000000, 500000, "WarpDriveTech", WarpDriveTechnologyRequirements, 10)

        ResearchList.Add("WeaponsTech", WeaponsTechnology) : ResearchList.Add("DefenceTech", DefenceTechnology) : ResearchList.Add("ShieldingTech", ShieldTechnology) : ResearchList.Add("EspionageTech", EspionageTechnology)
        ResearchList.Add("RocketEngineTech", RocketEngineTechnology) : ResearchList.Add("SolarEngineTech", SolarEngineTechnology) : ResearchList.Add("NuclearFusionTech", NuclearFusion)
        ResearchList.Add("ColonisationTech", ColonisationTechnology) : ResearchList.Add("HyperspaceEngineTech", HyperspaceEngineTechnology)
        ResearchList.Add("WarpDriveTech", WarpDriveTechnology) : ResearchList.Add("EnergyTech", EnergyTechology)


    End Sub

    Private Sub frmMain_ForeColorChanged(sender As Object, e As EventArgs) Handles Me.ForeColorChanged

    End Sub

    Public Function SQLSingleValue(sql As String) As String
        Dim sqlquery As String = sql
        Dim ConnectionString As String
        Dim Result As String = ""
        ConnectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alem2\Downloads\Dropbox\GameServer\GameServer\GameServer\GameServer\Database1.mdf;Integrated Security=True"

        'Connect
        Using conn As SqlConnection = New SqlConnection(ConnectionString)
            conn.Open()
            Using comm As SqlCommand = New SqlCommand(sqlquery, conn)
                Try
                    Result = Convert.ToString(comm.ExecuteScalar())
                Catch ex As SqlException
                    MessageBox.Show(ex.Message.ToString(), "Error Message")
                End Try

                conn.Close()
            End Using 'com
        End Using
        If Result = "" Then
            Result = "null"
        End If
        Return Result
    End Function

    Public Function GetDataTable(Sql As String) As DataTable
        'the DataTable to return
        Dim MyDataTable As New DataTable
        Dim ConnectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alem2\Downloads\Dropbox\GameServer\GameServer\GameServer\GameServer\Database1.mdf;Integrated Security=True"
        'make a SqlConnection using the supplied ConnectionString 
        Dim MySqlConnection As New SqlConnection(ConnectionString)
        Using MySqlConnection
            'make a query using the supplied Sql


            'open the connection
            MySqlConnection.Open()
            Using dad As New SqlDataAdapter(Sql, MySqlConnection)
                dad.Fill(MyDataTable)
                'create a DataReader and execute the SqlCommand
            End Using

            'load the reader into the datatable


            'clean up

        End Using
        MySqlConnection.Close()

        'return the datatable
        Return MyDataTable
    End Function
    Public Function InsertSQL(sql As String) As Boolean
        Using conn As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alem2\Downloads\Dropbox\GameServer\GameServer\GameServer\GameServer\Database1.mdf;Integrated Security=True")
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = sql
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As SqlException
                    MessageBox.Show(ex.Message.ToString(), "Error Message")
                    Return False
                End Try
            End Using
        End Using
        Return True

    End Function
    Private Sub SetUpStructures()
        StructureList.Add("MetalFoundry", New Structures(60, 15, 0))
        StructureList.Add("SiliconExtractor", New Structures(48, 24, 0))
        StructureList.Add("TritiumGenerator", New Structures(225, 75, 0))
        StructureList.Add("Shipyard", New Structures(100, 50, 20))
        StructureList.Add("ResearchLab", New Structures(150, 100, 60))
        StructureList.Add("RoboticsFactory", New Structures(250, 125, 75, 2))
    End Sub



End Class