Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Inventario

Namespace Servicios
    Public Class Movimiento_InventarioServicio

        Private ReadOnly _datos As Movimiento_InventarioDatos

        Public Sub New()
            _datos = New Movimiento_InventarioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Movimiento_Inventario) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Movimiento_Inventario)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Movimiento_Inventario
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Movimiento_Inventario)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Movimiento_Inventario)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace