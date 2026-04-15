Option Strict On
Option Explicit On

Namespace RecursosHumanos
    Public Class Empleado
        Public Property EmpId As Integer
        Public Property DeptoId As Integer
        Public Property CargoId As Integer
        Public Property RolEmpleadoId As Integer?
        Public Property Nombres As String
        Public Property Apellidos As String
        Public Property Email As String
        Public Property Telefono As String
        Public Property FechaIngreso As DateTime
        Public Property SalarioBase As Decimal?
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace