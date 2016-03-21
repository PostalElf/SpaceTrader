Public Structure damage
    Friend Sub New(ByVal aType As eDamageType, ByVal aAccuracy As Integer, ByVal aDamageFull As Integer, ByVal aDamageGlancing As Integer, Optional ByVal aDigitalPayload As eDigitalAttack = Nothing)
        type = aType
        accuracy = aAccuracy
        damageFull = aDamageFull
        damageGlancing = aDamageGlancing
        digitalPayload = aDigitalPayload
    End Sub

    Friend type As eDamageType
    Friend accuracy As Integer
    Friend damageFull As Integer
    Friend damageGlancing As Integer
    Friend digitalPayload As eDigitalAttack
End Structure
