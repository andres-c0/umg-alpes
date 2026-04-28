Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class Materia_PrimaServicio

        Private ReadOnly _datos As Materia_PrimaDatos

        Public Sub New()
            _datos = New Materia_PrimaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Materia_Prima) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Materia_Prima)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Materia_Prima
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Materia_Prima)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Materia_Prima)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace