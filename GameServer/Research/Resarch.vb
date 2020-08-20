Public Class Research
    Private resmetalcost As Integer
    Private ressiliconcost As Integer
    Private restritiumcost As Integer
    Private resname As String
    Private resrequirements As New Dictionary(Of String, Integer)
    Private researchlablevelrequired As Integer
    Sub New(ByVal metalcost As Integer, ByVal siliconcost As Integer, ByVal tritiumcost As Integer, ByVal name As String, ByVal requirements As Dictionary(Of String, Integer), ByVal ResearchLabLevel As Integer)

        resmetalcost = metalcost
        ressiliconcost = siliconcost
        restritiumcost = tritiumcost

        resname = name
        resrequirements = requirements
        researchlablevelrequired = ResearchLabLevel
    End Sub

    Sub GetCost(level As Integer, ByRef MetalCost As Integer, ByRef SiliconCost As Integer, ByRef TritiumCost As Integer)
        MetalCost = resmetalcost * (2 ^ level)
        SiliconCost = ressiliconcost * (2 ^ level)
        TritiumCost = restritiumcost * (2 ^ level)
    End Sub
    Function GetName()
        Return resname
    End Function
    Function GetRequirements()
        Return resrequirements
    End Function
    Function GetResearchLabLevelRequired()
        Return researchlablevelrequired
    End Function
End Class
