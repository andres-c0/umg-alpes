Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RRHH

Namespace Servicios
    Public Class ExpedienteEmpleadoServicio

        Private ReadOnly _datos As ExpedienteEmpleadoDatos

        Public Sub New()
            _datos = New ExpedienteEmpleadoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ExpedienteEmpleado) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As ExpedienteEmpleado)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ExpedienteEmpleado
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of ExpedienteEmpleado)
            Return _datos.Listar()
        End Function

    End Class
End Namespace