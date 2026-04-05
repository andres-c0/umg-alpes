Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RecursosHumanos

Namespace Servicios
    Public Class Expediente_EmpleadoServicio

        Private ReadOnly _datos As Expediente_EmpleadoDatos

        Public Sub New()
            _datos = New Expediente_EmpleadoDatos()
        End Sub

        Public Function Listar() As List(Of Expediente_Empleado)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Expediente_Empleado
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Insertar(ByVal entidad As Expediente_Empleado) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Expediente_Empleado)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace