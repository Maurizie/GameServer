Imports System.IO
Imports System.Data.SqlClient
Public Class Player
    Private ID As String
    Private TutComplete As Boolean
    Private Tritium As Double
    Private Silicon As Double
    Private Metal As Double
    Private filepath As String
    Private LastDate As Date
    Private MFLevel As Integer
    Private SELevel As Integer
    Private TGLevel As Integer
    Private RLLevel As Integer
    Private SYLevel As Integer
    Private RFLevel As Integer
    Private ResearchLevels As New Dictionary(Of String, Integer)
    Private StructureLevels As New Dictionary(Of String, Integer)


    Sub New(ByVal UserID As String)
        ID = UserID
        TutComplete = True

        'Dim dt As New DataTable
        'Dim da As New dbConn
        'da.strSQL = "select top 1 * from [Player] where [User] = '" & ID.ToLower & "'"
        'dt = da.getDt
        'filepath = "C:\ProjGame" & "\" & ID.ToLower & "\basics.txt"
        'Dim filedata() As String = File.ReadAllLines(filepath)

        Dim sql As String = "select * from Player where [User] = '" & ID & "'"
        Dim PlayerInfo As DataTable = frmMain.GetDataTable(sql)


        '''''' LastDate = CDate(filedata(9))
        LastDate = PlayerInfo.Rows(0).Item("Login")
        ''''''''''StructureLevels("Metal Foundry") = Mid(filedata(11), 4, filedata(11).Length - 3)
        StructureLevels.Add("MetalFoundry", PlayerInfo.Rows(0).Item("MetalFoundry"))
        ''''''''''SELevel = Mid(filedata(12), 4, filedata(12).Length - 3)
        StructureLevels.Add("SiliconExtractor", PlayerInfo.Rows(0).Item("SiliconExtractor"))
        ''''''''''TGLevel = Mid(filedata(13), 4, filedata(13).Length - 3)
        StructureLevels.Add("TritiumGenerator", PlayerInfo.Rows(0).Item("TritiumGenerator"))
        ''''''''''RLLevel = Mid(filedata(14), 4, filedata(14).Length - 3)
        StructureLevels.Add("ResearchLab", PlayerInfo.Rows(0).Item("ResearchLab"))
        ''''''''''RFLevel = Mid(filedata(15), 4, filedata(15).Length - 3)
        StructureLevels.Add("RoboticsFactory", PlayerInfo.Rows(0).Item("RoboticsFactory"))
        ''''''''''SYLevel = Mid(filedata(16), 4, filedata(16).Length - 3)
        StructureLevels.Add("Shipyard", PlayerInfo.Rows(0).Item("Shipyard"))

        ResearchLevels.Add("WeaponsTech", CInt(PlayerInfo.Rows(0).Item("WeaponsTech")))
        ResearchLevels.Add("DefenceTech", CInt(PlayerInfo.Rows(0).Item("DefenceTech")))
        ResearchLevels.Add("ShieldingTech", CInt(PlayerInfo.Rows(0).Item("ShieldingTech")))
        ResearchLevels.Add("EspionageTech", CInt(PlayerInfo.Rows(0).Item("EspionageTech")))
        ResearchLevels.Add("RocketEngineTech", CInt(PlayerInfo.Rows(0).Item("RocketEngineTech")))
        ResearchLevels.Add("SolarEngineTech", CInt(PlayerInfo.Rows(0).Item("SolarEngineTech")))
        ResearchLevels.Add("NuclearFusionTech", CInt(PlayerInfo.Rows(0).Item("NuclearFusionTech")))
        ResearchLevels.Add("ColonisationTech", CInt(PlayerInfo.Rows(0).Item("ColonisationTech")))
        ResearchLevels.Add("HyperspaceEngineTech", CInt(PlayerInfo.Rows(0).Item("HyperspaceEngineTech")))
        ResearchLevels.Add("WarpDriveTech", CInt(PlayerInfo.Rows(0).Item("WarpDriveTech")))
        ResearchLevels.Add("EnergyTech", CInt(PlayerInfo.Rows(0).Item("EnergyTech")))


        'Dim sqlquery As String
        'Dim ConnectionString As String
        'ConnectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\alem2\Downloads\Dropbox\GameServer\GameServer\GameServer\GameServer\Database1.mdf;Integrated Security=True"
        'sqlquery = "SELECT [Metal] FROM [Player] where [User] = '" & ID.ToLower & "'"

        ''Connect
        'Using conn As SqlConnection = New SqlConnection(ConnectionString)
        '    conn.Open()
        '    Using comm As SqlCommand = New SqlCommand(sqlquery, conn)
        '        Try
        '            Metal = Convert.ToDouble(comm.ExecuteScalar())
        '        Catch ex As SqlException
        '            MessageBox.Show(ex.Message.ToString(), "Error Message")
        '        End Try

        '        conn.Close()
        '    End Using 'com
        'End Using

        'Metal = CDbl(filedata(5).Substring(2, filedata(5).Length - 2))
        Metal = PlayerInfo.Rows(0).Item("Metal")
        CalculateMaterials()

        Silicon = PlayerInfo.Rows(0).Item("Silicon")
        CalculateSilicon()

        Tritium = PlayerInfo.Rows(0).Item("Tritium")
        CalculateTritium()


    End Sub

    Sub CalculateMaterials()
        'Dim filedata() As String = File.ReadAllLines(filepath)

        Dim time As System.TimeSpan
        Dim currenttime = DateTime.Now
        time = currenttime.Subtract(LastDate)
        Dim seconds As Integer = time.Seconds + (time.Minutes * 60) + (time.Hours * 3600) + (time.Days * 3600 * 24)

        Metal += seconds * CalculateMetalRate()

        Silicon += seconds * CalculateSiliconRate()

        Tritium += seconds * CalculateTritiumRate()

        LastDate = DateTime.Now
        Dim Sql = String.Format("Update Player set Metal = '{0}', Silicon = '{1}', Tritium = '{2}' , Login = '{3}' where [User] = '{4}'", CLng(Metal), CLng(Silicon), CLng(Tritium), CStr(LastDate), ID)


        frmMain.InsertSQL(sql)
        ' filedata(5) = "m " & Metal.ToString("0.00")
        '  File.WriteAllLines(filepath, filedata)

    End Sub

    Sub CalculateSilicon()
        '  Dim filedata() As String = File.ReadAllLines(filepath)
        Dim time As System.TimeSpan
        Dim currenttime = DateTime.Now
        time = currenttime.Subtract(LastDate)
        Dim seconds As Integer = time.Seconds + (time.Minutes * 60) + (time.Hours * 3600) + (time.Days * 3600 * 24)

        Silicon += seconds * CalculateSiliconRate()
        Dim sql As String = "Update Player set Silicon = '" & CLng(Silicon) & "' where [User] = '" & ID & "'"
        frmMain.InsertSQL(sql)
    End Sub

    Sub CalculateTritium()
        '  Dim filedata() As String = File.ReadAllLines(filepath)
        Dim time As System.TimeSpan
        Dim currenttime = DateTime.Now
        time = currenttime.Subtract(LastDate)
        Dim seconds As Integer = time.Seconds + (time.Minutes * 60) + (time.Hours * 3600) + (time.Days * 3600 * 24)


        LastDate = DateTime.Now
        Dim sql As String = "Update Player set Tritium = '" & CLng(Tritium) & "' , Login = '" & CStr(LastDate) & "' where [User] = '" & ID & "'"
        frmMain.InsertSQL(sql)
    End Sub

    Private Function CalculateMetalRate()
        Dim rate As Single
        rate = (30 * StructureLevels("MetalFoundry") * (1.1 ^ StructureLevels("MetalFoundry"))) / 3600
        Return rate
    End Function

    Private Function CalculateSiliconRate()
        Dim rate As Single
        rate = (20 * StructureLevels("SiliconExtractor") * (1.1 ^ StructureLevels("SiliconExtractor"))) / 3600
        Return rate
    End Function

    Private Function CalculateTritiumRate()
        Dim rate As Single
        rate = (10 * StructureLevels("TritiumGenerator") * (1.1 ^ StructureLevels("TritiumGenerator"))) / 3600
        Return rate
    End Function

    Function GetMetal() As Double
        CalculateMaterials()
        Return Metal
    End Function
    Function GetSIlicon() As Double
        CalculateSilicon()
        Return Silicon
    End Function
    Function GetTritium() As Double
        CalculateTritium()
        Return Tritium
    End Function

    Function GetMFLevel() As Integer
        Return MFLevel
    End Function
    Function GetSELevel() As Integer
        Return SELevel
    End Function
    Function GetTGLevel() As Integer
        Return TGLevel
    End Function
    Function GetRFLevel() As Integer
        Return RFLevel
    End Function
    Function GetRLLevel() As Integer
        Return RLLevel
    End Function
    Function GetSYLevel() As Integer
        Return SYLevel
    End Function


    Sub ChangeLoginData()
        ' Dim filedata() As String = File.ReadAllLines(filepath)
        CalculateMaterials()

        ''LastDate = DateTime.Now
        ' filedata(9) = LastDate
        'File.WriteAllLines(filepath, filedata)
    End Sub

    Sub UpgradeMF()
        Dim MetalUpgradeCost, SiliconUpgradeCost As Integer
        CalculateMaterials()
        MetalUpgradeCost = (60 * 1.5 ^ (StructureLevels("MetalFoundry") - 1))
        SiliconUpgradeCost = (15 * 1.5 ^ (StructureLevels("MetalFoundry") - 1))
        If Metal > MetalUpgradeCost And Silicon > SiliconUpgradeCost Then
            Metal = Metal - MetalUpgradeCost
            Silicon = Silicon - SiliconUpgradeCost
            StructureLevels("MetalFoundry") += 1
            Dim sql As String = "update Player set MF = '" & MFLevel & "' where [User] = '" & ID.ToLower & "'"
            frmMain.InsertSQL(sql)
            ' Dim filedata() As String = File.ReadAllLines(filepath)
            'filedata(11) = "mf " & MFLevel
            'File.WriteAllLines(filepath, filedata)

        End If
        CalculateMaterials()


    End Sub

    Sub UpgradeSE()
        Dim MetalUpgradeCost, SiliconUpgradeCost As Integer
        CalculateMaterials()
        MetalUpgradeCost = (48 * 1.5 ^ (SELevel - 1))
        SiliconUpgradeCost = (24 * 1.5 ^ (SELevel - 1))
        If Metal > MetalUpgradeCost And Silicon > SiliconUpgradeCost Then
            Metal = Metal - MetalUpgradeCost
            Silicon = Silicon - SiliconUpgradeCost
            SELevel += 1
            Dim filedata() As String = File.ReadAllLines(filepath)
            filedata(12) = "se " & SELevel
            File.WriteAllLines(filepath, filedata)
        End If
        CalculateMaterials()
    End Sub

    Sub UpgradeTG()
        Dim MetalUpgradeCost, SiliconUpgradeCost As Integer
        CalculateMaterials()
        MetalUpgradeCost = (225 * 1.5 ^ (TGLevel - 1))
        SiliconUpgradeCost = (75 * 1.5 ^ (TGLevel - 1))
        If Metal > MetalUpgradeCost And Silicon > SiliconUpgradeCost Then
            Metal = Metal - MetalUpgradeCost
            Silicon = Silicon - SiliconUpgradeCost
            TGLevel += 1
            Dim filedata() As String = File.ReadAllLines(filepath)
            filedata(13) = "tg " & TGLevel
            File.WriteAllLines(filepath, filedata)
        End If
        CalculateMaterials()
    End Sub

    Sub UpgradeSY()
        Dim MetalUpgradeCost, SiliconUpgradeCost, TritiumUpgradeCost As Integer
        CalculateMaterials()
        MetalUpgradeCost = (100 * 1.5 ^ (SYLevel - 1))
        SiliconUpgradeCost = (50 * 1.5 ^ (SYLevel - 1))
        TritiumUpgradeCost = (20 * 1.5 ^ (SYLevel - 1))

        If Metal > MetalUpgradeCost And Silicon > SiliconUpgradeCost And Tritium > TritiumUpgradeCost Then
            Metal = Metal - MetalUpgradeCost
            Silicon = Silicon - SiliconUpgradeCost
            Tritium = Tritium - TritiumUpgradeCost
            SYLevel += 1
            Dim filedata() As String = File.ReadAllLines(filepath)
            filedata(16) = "sy " & SYLevel
            File.WriteAllLines(filepath, filedata)
        End If
        CalculateMaterials()
    End Sub

    Sub UpgradeRL()
        Dim MetalUpgradeCost, SiliconUpgradeCost, TritiumUpgradeCost As Integer
        CalculateMaterials()
        MetalUpgradeCost = (150 * 1.5 ^ (RLLevel - 1))
        SiliconUpgradeCost = (100 * 1.5 ^ (RLLevel - 1))
        TritiumUpgradeCost = (60 * 1.5 ^ (RLLevel - 1))

        If Metal > MetalUpgradeCost And Silicon > SiliconUpgradeCost And Tritium > TritiumUpgradeCost Then
            Metal = Metal - MetalUpgradeCost
            Silicon = Silicon - SiliconUpgradeCost
            Tritium = Tritium - TritiumUpgradeCost
            RLLevel += 1
            Dim filedata() As String = File.ReadAllLines(filepath)
            filedata(14) = "rl " & RLLevel
            File.WriteAllLines(filepath, filedata)
        End If
        CalculateMaterials()
    End Sub

    Sub UpgradeRF()
        Dim MetalUpgradeCost, SiliconUpgradeCost, TritiumUpgradeCost As Integer
        CalculateMaterials()
        MetalUpgradeCost = (250 * (2 ^ (RFLevel - 1)))
        SiliconUpgradeCost = (125 * (2 ^ (RFLevel - 1)))
        TritiumUpgradeCost = (75 * (2 ^ (RFLevel - 1)))

        If Metal > MetalUpgradeCost And Silicon > SiliconUpgradeCost And Tritium > TritiumUpgradeCost Then
            Metal = Metal - MetalUpgradeCost
            Silicon = Silicon - SiliconUpgradeCost
            Tritium = Tritium - TritiumUpgradeCost
            RFLevel += 1
            Dim filedata() As String = File.ReadAllLines(filepath)
            filedata(15) = "rf " & RFLevel
            File.WriteAllLines(filepath, filedata)
        End If
        CalculateMaterials()
    End Sub

    Sub ResearchTechnology(ByVal research As String)
        Dim metalcost, siliconcost, tritiumcost, researchlablevelrequired As Integer
        Dim requirements As List(Of Research)
        CalculateMaterials()
        frmMain.ResearchList(research).GetCost(1, metalcost, siliconcost, tritiumcost)
        researchlablevelrequired = frmMain.ResearchList(research).GetResearchLabLevelRequired
        requirements = frmMain.ResearchList(research).GetRequirements

        If requirements.Count <> 0 Then
            Dim researchrequired(0) As Research
            requirements.CopyTo(researchrequired)
            Dim nameofresearchrequired As String = researchrequired(0).GetName
            Select Case LCase(nameofresearchrequired)
                Case "rocketenginetech"
                    If ResearchLevels("FuelEfficiencyTech") < 2 Then
                        frmMain.SendMessage(ID, "research", "toolow")
                        Return
                    End If
                Case "solarenginetechnology"
                    If ResearchLevels("FuelEfficiencyTechnology") < 6 Then
                        frmMain.SendMessage(ID, "research", "toolow")
                        Return
                    End If
                Case "nuclearfissiontechnology"
                    If ResearchLevels("FuelEfficiencyTechnology") < 5 Then
                        frmMain.SendMessage(ID, "research", "toolow")
                        Return
                    End If
                Case "colonisationtechnology"
                    If ResearchLevels("SolarEngineTechnology") < 4 Then
                        frmMain.SendMessage(ID, "research", "toolow")
                        Return
                    End If
                Case "hyperspaceenginetechnology"
                    If ResearchLevels("NuclearFusionTechnology") < 4 Then
                        frmMain.SendMessage(ID, "research", "toolow")
                        Return
                    End If
                Case "warpdrivetechnology"
                    If ResearchLevels("HyperspaceEngineTechnology") < 4 Then
                        frmMain.SendMessage(ID, "research", "toolow")
                        Return
                    End If
            End Select
        End If
        Dim currentresearchlevel As Integer = ResearchLevels(research)
        If Metal < (metalcost * (2 ^ currentresearchlevel)) Or Silicon < (siliconcost * (2 ^ currentresearchlevel)) Or Tritium < (tritiumcost * (2 ^ currentresearchlevel)) Then
            frmMain.SendMessage(ID, "research", "toolowres")
            Return
        End If
        If RLLevel < researchlablevelrequired Then
            frmMain.SendMessage(ID, "research", "toolowrl")
            Return
        End If
        ResearchLevels(research) += 1
        Metal -= (metalcost * (2 ^ currentresearchlevel))
        Silicon -= (siliconcost * (2 ^ currentresearchlevel))
        Tritium -= (tritiumcost * (2 ^ currentresearchlevel))

        CalculateMaterials()

        WriteNewResearchesToFile()
    End Sub
    Sub WriteNewResearchesToFile()
        'Dim filedata() As String = File.ReadAllLines(filepath)
        'filedata(18) = "wt " & ResearchLevels("WeaponsTechnology")
        'filedata(19) = "dt " & ResearchLevels("DefenceTechnology")
        'filedata(20) = "st " & ResearchLevels("ShieldingTechnology")
        'filedata(21) = "fe " & ResearchLevels("FuelEfficiencyTechnology")
        'filedata(22) = "re " & ResearchLevels("RocketEngineTechnology")
        'filedata(23) = "se " & ResearchLevels("SolarEngineTechnology")
        'filedata(24) = "nf " & ResearchLevels("NuclearFusionTechnology")
        'filedata(25) = "ct " & ResearchLevels("ColonisationTechnology")
        'filedata(26) = "he " & ResearchLevels("HyperspaceEngineTechnology")
        'filedata(27) = "wd " & ResearchLevels("WarpDriveTechnology")

        'File.WriteAllLines(filepath, filedata)

    End Sub

    Sub RequestResearchLevels(ByVal research As String)
        Select Case Mid(research, 4)
            Case "wep"
                research = "WeaponsTechnology"
                SendResearch(research)
            Case "shield"
                research = "ShieldingTechnology"
                SendResearch(research)
            Case "armour"
                research = "DefenceTechnology"
                SendResearch(research)
            Case "fuel"
                research = "FuelEfficiencyTechnology"
                SendResearch(research)
            Case "rocket"
                research = "RocketEngineTechnology"
                SendResearch(research)
            Case "solar"
                research = "SolarEngineTechnology"
                SendResearch(research)
            Case "fusion"
                research = "NuclearFusionTechnology"
                SendResearch(research)
            Case "colonisation"
                research = "ColonisationTechnology"
                SendResearch(research)
            Case "hyperspace"
                research = "HyperspaceEngineTechnology"
                SendResearch(research)
            Case "warp"
                research = "WarpDriveTechnology"
                SendResearch(research)
        End Select
    End Sub

    Sub SendResearch(ByVal resarch As String)
        frmMain.SendMessage(ID, "research", "send" & resarch & "@" & ResearchLevels(resarch))
    End Sub

    Sub CheckResearchCost(ByVal research As String)
        Dim researchrequired As String = Mid(research, 6)
        Dim currentlevel As Integer
        currentlevel = ResearchLevels(researchrequired)
        Dim metalcost, siliconcost, tritiumcost As Integer
        frmMain.ResearchList(research).GetCost(ResearchLevels(research), metalcost, siliconcost, tritiumcost)

        frmMain.SendMessage(ID, "research", "info" & researchrequired & "@" & metalcost & "@" & siliconcost & "@" & tritiumcost)
    End Sub


    Sub UpgradeBuilding(ByVal BuildingType As String)
        Dim MetalUpgradeCost, SiliconUpgradeCost, TritiumUpgradeCost As Integer
        Dim nextlevel As Integer = StructureLevels(BuildingType) + 1
        CalculateMaterials()
        frmMain.StructureList(BuildingType).CalculateCost(nextlevel - 1, MetalUpgradeCost, SiliconUpgradeCost, TritiumUpgradeCost)

        If Metal >= MetalUpgradeCost And Silicon >= SiliconUpgradeCost And Tritium >= TritiumUpgradeCost Then
            Metal -= MetalUpgradeCost
            Silicon -= SiliconUpgradeCost
            Tritium -= TritiumUpgradeCost
            StructureLevels(BuildingType) += 1
            Dim sql As String = "update Player set " & BuildingType & " = '" & nextlevel & "' where [User] = '" & ID.ToLower & "'"
            frmMain.InsertSQL(sql)
            ' Dim filedata() As String = File.ReadAllLines(filepath)
            'filedata(11) = "mf " & MFLevel
            'File.WriteAllLines(filepath, filedata)

        End If
        CalculateMaterials()

    End Sub


    Function EncodeStructureLevels() As String
        Dim returnString As String = ""
        returnString &= StructureLevels("MetalFoundry").ToString() & "\" _
        & StructureLevels("SiliconExtractor").ToString() & "\" _
        & StructureLevels("TritiumGenerator").ToString() & "\" _
        & StructureLevels("ResearchLab").ToString() & "\" _
        & StructureLevels("Shipyard").ToString() & "\" _
        & StructureLevels("RoboticsFactory").ToString()
        Return returnString


    End Function

    Function EncodeResources() As String
        CalculateMaterials()
        Dim returnString As String = ""
        returnString &= Metal.ToString() & "\" _
        & Silicon.ToString() & "\" _
        & Tritium.ToString()
        Return returnString
    End Function

End Class
