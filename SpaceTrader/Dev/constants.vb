Public Class constants
    Friend Shared ReadOnly Property resourceArray As Array
        Get
            Return System.Enum.GetValues(GetType(eResource))
        End Get
    End Property
    Friend Shared ReadOnly Property saleHullComponentArray As Array
        Get
            Return System.Enum.GetValues(GetType(eHcCategory))
        End Get
    End Property
    Friend Shared ReadOnly Property planetRoleArray As Array
        Get
            Return System.Enum.GetValues(GetType(ePlanetRole))
        End Get
    End Property
    Friend Shared ReadOnly Property planetTypeArray As Array
        Get
            Return System.Enum.GetValues(GetType(ePlanetType))
        End Get
    End Property
    Friend Shared ReadOnly Property damageTypeArray As Array
        Get
            Return System.Enum.GetValues(GetType(eDamageType))
        End Get
    End Property
    Friend Shared ReadOnly Property defenceTypeArray As Array
        Get
            Return System.Enum.GetValues(GetType(eDefenceType))
        End Get
    End Property
    Friend Shared ReadOnly Property raceArray As Array
        Get
            Return System.Enum.GetValues(GetType(eRace))
        End Get
    End Property
    Friend Shared ReadOnly Property hcTypeList As List(Of Type)
        Get
            Dim total As New List(Of Type)
            With total
                .Add(GetType(hcCargo))
                .Add(GetType(hcCrewQuarters))
                .Add(GetType(hcDefence))
                .Add(GetType(hcEngine))
                .Add(GetType(hcJumpDrive))
                .Add(GetType(hcProducer))
                .Add(GetType(hcWeapon))
            End With
            Return total
        End Get
    End Property

    Friend Shared Function getEnumFromString(ByVal str As String, ByVal enumArray As Array) As [Enum]
        For Each item In enumArray
            Dim itemStr As String = item.ToString.ToLower

            If itemStr = str.ToLower Then Return item
            If stripS(itemStr.ToLower) = str.ToLower Then Return item
            If stripS(str.ToLower) = itemStr.ToLower Then Return item
        Next
        Return Nothing
    End Function
    Friend Shared Function getServiceFromTypeString(ByVal str As String) As eHcCategory
        If str = Nothing Then
            MsgBox("getServiceFromTypeString exception")
            Return Nothing
        End If

        Select Case str.ToLower
            Case "cargo", "crewquarters" : Return eHcCategory.Storage
            Case "defence", "weapon" : Return eHcCategory.War
            Case "engine", "jumpdrive" : Return eHcCategory.Travel
            Case "producer" : Return eHcCategory.Production
            Case Else
                MsgBox("getServiceFromTypeString exception")
                Return Nothing
        End Select
    End Function
End Class
