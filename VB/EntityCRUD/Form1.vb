Imports Microsoft.VisualBasic
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
					Dim newPerson As Person = New Person With {.FirstName = "First Name" & i, .LastName = "Last Name" & i}
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
			Dim outp As String = String.Format("{0} added; {1} deleted; {2} modified",addedCount,deletedCount,changedCount)
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

		Private privatePersons As DbSet(Of Person)
		Public Property Persons() As DbSet(Of Person)
			Get
				Return privatePersons
			End Get
			Set(ByVal value As DbSet(Of Person))
				privatePersons = value
			End Set
		End Property
		Private privateAdresses As DbSet(Of Adress)
		Public Property Adresses() As DbSet(Of Adress)
			Get
				Return privateAdresses
			End Get
			Set(ByVal value As DbSet(Of Adress))
				privateAdresses = value
			End Set
		End Property

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

		Private privateFirstName As String
		Public Property FirstName() As String
			Get
				Return privateFirstName
			End Get
			Set(ByVal value As String)
				privateFirstName = value
			End Set
		End Property
		Private privateLastName As String
		Public Property LastName() As String
			Get
				Return privateLastName
			End Get
			Set(ByVal value As String)
				privateLastName = value
			End Set
		End Property
		Private privateID As Integer
		Public Property ID() As Integer
			Get
				Return privateID
			End Get
			Set(ByVal value As Integer)
				privateID = value
			End Set
		End Property
		Private privateAdresses As ICollection(Of Adress)
		Public Overridable Property Adresses() As ICollection(Of Adress)
			Get
				Return privateAdresses
			End Get
			Set(ByVal value As ICollection(Of Adress))
				privateAdresses = value
			End Set
		End Property
	End Class
	Public Class Adress
		Private privateCity As String
		Public Property City() As String
			Get
				Return privateCity
			End Get
			Set(ByVal value As String)
				privateCity = value
			End Set
		End Property
		Private privateID As Integer
		Public Property ID() As Integer
			Get
				Return privateID
			End Get
			Set(ByVal value As Integer)
				privateID = value
			End Set
		End Property
		Private privatePersonID? As Integer
		Public Property PersonID() As Integer?
			Get
				Return privatePersonID
			End Get
			Set(ByVal value? As Integer)
				privatePersonID = value
			End Set
		End Property
		Private privatePerson As Person
		<Required> _
		Public Overridable Property Person() As Person
			Get
				Return privatePerson
			End Get
			Set(ByVal value As Person)
				privatePerson = value
			End Set
		End Property
		Public Sub New()

		End Sub
	End Class
End Namespace
