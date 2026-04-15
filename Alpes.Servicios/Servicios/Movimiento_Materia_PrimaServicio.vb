Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class Movimiento_Materia_PrimaServicio

        Private ReadOnly _datos As Movimiento_Materia_PrimaDatos

        Public Sub New()
            _datos = New Movimiento_Materia_PrimaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Movimiento_Materia_Prima) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Movimiento_Materia_Prima)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Movimiento_Materia_Prima
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Movimiento_Materia_Prima)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Movimiento_Materia_Prima)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace