Public Class constants
    Friend Shared ReadOnly Property resourceArray As Array
        Get
            Return System.Enum.GetValues(GetType(eResource))
        End Get
    End Property
    Friend Shared ReadOnly Property planetPrefixArray As Array
        Get
            Return System.Enum.GetValues(GetType(ePlanetRole))
        End Get
    End Property
    Friend Shared ReadOnly Property planetSuffixArray As Array
        Get
            Return System.Enum.GetValues(GetType(ePlanetType))
        End Get
    End Property
    Friend Shared ReadOnly Property defenceTypearray As Array
        Get
            Return System.Enum.GetValues(GetType(eDefenceType))
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
    Friend Shared ReadOnly Property defaultProductsPrices As Dictionary(Of eResource, Integer)
        Get
            Dim total As New Dictionary(Of eResource, Integer)
            With total
                .Add(eResource.Metals, 100)
                .Add(eResource.Chemicals, 100)
                .Add(eResource.Ammunition, 50)
                .Add(eResource.Missiles, 70)
                .Add(eResource.Savants, 150)
                .Add(eResource.Machines, 100)
                .Add(eResource.Slaves, 100)
                .Add(eResource.Azoth, 200)
                .Add(eResource.Food, 50)
                .Add(eResource.Organics, 50)
                .Add(eResource.Bandwidth, 120)
                .Add(eResource.Media, 100)
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
End Class
