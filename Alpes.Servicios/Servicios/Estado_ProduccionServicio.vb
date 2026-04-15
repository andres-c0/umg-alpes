Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class Estado_ProduccionServicio

        Private ReadOnly _datos As Estado_ProduccionDatos

        Public Sub New()
            _datos = New Estado_ProduccionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Estado_Produccion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Estado_Produccion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Estado_Produccion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Estado_Produccion)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Estado_Produccion)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace