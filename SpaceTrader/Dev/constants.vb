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
