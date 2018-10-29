Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Data.Entity
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace EntityCRUD
    Partial Public Class Form1
        Inherits Form

        Private _context As New MyContext()
        Public Sub New()
            InitializeComponent()
            gridView1.DataController.AllowIEnumerableDetails = True
            FillData()
            _context.Persons.Load()
            gridControl1.DataSource = _context.Persons.Local.ToBindingList()
        End Sub

        Private Shared Sub FillData()
            Using _cnt = New MyContext()
                _cnt.Configuration.AutoDetectChangesEnabled = False
                If _cnt.Persons.Any() Then
                    Return
                End If
                For i As Integer = 0 To 9
                    Dim newPerson As Person = New Person With { _
                        .FirstName = "First Name" & i, _
                        .LastName = "Last Name" & i _
                    }
                    Dim adr As New Adress()
                    adr.City = "City" & i
                    newPerson.Adresses.Add(adr)
                    _cnt.Persons.Add(newPerson)
                Next i
                _cnt.SaveChanges()
            End Using
        End Sub
        Private Sub previewChanges()
            Dim changeSet = _context.ChangeTracker.Entries()
            Dim deletedCount As Integer = 0
            Dim changedCount As Integer = 0
            Dim addedCount As Integer = 0
            If changeSet IsNot Nothing Then
                For Each dbEntityEntry In changeSet
                    Select Case dbEntityEntry.State
                        Case EntityState.Deleted
                            deletedCount += 1
                            ' process
                        Case EntityState.Unchanged
                        Case EntityState.Added
                            addedCount += 1
                            ' process
                        Case EntityState.Modified
                            changedCount += 1
                    End Select

                Next dbEntityEntry
            End If
            Dim outp As String = String.Format("{0} added; {1} deleted; {2} modified", addedCount, deletedCount, changedCount)
            MessageBox.Show(outp)
        End Sub

        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
            previewChanges()
        End Sub

        Private Sub simpleButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton2.Click
            _context.SaveChanges()
        End Sub
    End Class
    Public Class MyContext
        Inherits DbContext

        Public Sub New()
            MyBase.New("Data Source=(localdb)\v11.0;Initial Catalog=TestContext;Integrated Security=True;MultipleActiveResultSets=True;Application Name=TestOfMasterDetail3")
        End Sub

        Public Property Persons() As DbSet(Of Person)
        Public Property Adresses() As DbSet(Of Adress)

        Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
            MyBase.OnModelCreating(modelBuilder)
            modelBuilder.Entity(Of Adress)().Property(Function(a) a.PersonID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
            modelBuilder.Entity(Of Adress)().HasRequired(Function(a) a.Person).WithMany(Function(p) p.Adresses).HasForeignKey(Function(a) a.PersonID)
        End Sub
    End Class
    Public Class Person
        Public Sub New()
            Adresses = New List(Of Adress)()
        End Sub

        Public Property FirstName() As String
        Public Property LastName() As String
        Public Property ID() As Integer
        Public Overridable Property Adresses() As ICollection(Of Adress)
    End Class
    Public Class Adress
        Public Property City() As String
        Public Property ID() As Integer
        Public Property PersonID() As Integer?
        <Required> _
        Public Overridable Property Person() As Person
        Public Sub New()

        End Sub
    End Class
End Namespace
