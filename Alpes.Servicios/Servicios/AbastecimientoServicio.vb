Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class AbastecimientoServicio

        Private ReadOnly _datos As AbastecimientoDatos

        Public Sub New()
            _datos = New AbastecimientoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Abastecimiento) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As Abastecimiento) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal abastecimientoId As Integer) As Boolean
            Return _datos.Eliminar(abastecimientoId)
        End Function

        Public Function ObtenerPorId(ByVal abastecimientoId As Integer) As Abastecimiento
            Return _datos.ObtenerPorId(abastecimientoId)
        End Function

        Public Function Listar() As List(Of Abastecimiento)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Abastecimiento)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace