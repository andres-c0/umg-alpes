Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Inventario

Namespace Servicios
    Public Class Unidad_MedidaServicio

        Private ReadOnly _datos As Unidad_MedidaDatos

        Public Sub New()
            _datos = New Unidad_MedidaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Unidad_Medida) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Unidad_Medida)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Unidad_Medida
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Unidad_Medida)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Unidad_Medida)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace