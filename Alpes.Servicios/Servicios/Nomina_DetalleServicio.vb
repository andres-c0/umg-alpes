Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RecursosHumanos

Namespace Servicios
    Public Class Nomina_DetalleServicio

        Private ReadOnly _datos As Nomina_DetalleDatos

        Public Sub New()
            _datos = New Nomina_DetalleDatos()
        End Sub

        Public Function Listar() As List(Of Nomina_Detalle)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Nomina_Detalle
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Insertar(ByVal entidad As Nomina_Detalle) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Nomina_Detalle)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace