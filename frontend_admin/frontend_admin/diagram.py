# Crear un nuevo objeto Digraph para el diagrama de clases extendido
dot = Digraph(comment="Diagrama de Clases - Proyecto MVC con Controladores")

# Nodos para los modelos
dot.node("ExamenConfiguracion", '''<<TABLE BORDER="1" CELLBORDER="1" CELLSPACING="0">
  <TR><TD BGCOLOR="lightblue"><B>ExamenConfiguracion</B></TD></TR>
  <TR><TD>+ Id : int</TD></TR>
  <TR><TD>+ NumeroDisparos : int</TD></TR>
</TABLE>>''', shape='plaintext')

dot.node("Usuario", '''<<TABLE BORDER="1" CELLBORDER="1" CELLSPACING="0">
  <TR><TD BGCOLOR="lightblue"><B>Usuario</B></TD></TR>
  <TR><TD>+ IdUsuario : int</TD></TR>
  <TR><TD>+ Nombre : string?</TD></TR>
  <TR><TD>+ Correo : string?</TD></TR>
  <TR><TD>+ Clave : string?</TD></TR>
  <TR><TD>+ DNI : string?</TD></TR>
</TABLE>>''', shape='plaintext')

dot.node("ImpactosBala", '''<<TABLE BORDER="1" CELLBORDER="1" CELLSPACING="0">
  <TR><TD BGCOLOR="lightblue"><B>ImpactosBala</B></TD></TR>
  <TR><TD>+ IdImpacto : int</TD></TR>
  <TR><TD>+ IdUsuario : int</TD></TR>
  <TR><TD>+ Fecha : DateTime</TD></TR>
  <TR><TD>+ Ubicacion : string?</TD></TR>
  <TR><TD>+ Precision : float</TD></TR>
</TABLE>>''', shape='plaintext')

dot.node("ExternalUser", '''<<TABLE BORDER="1" CELLBORDER="1" CELLSPACING="0">
  <TR><TD BGCOLOR="lightblue"><B>ExternalUser</B></TD></TR>
  <TR><TD>+ dni : string?</TD></TR>
  <TR><TD>+ name : string?</TD></TR>
  <TR><TD>+ face_descriptor : string?</TD></TR>
</TABLE>>''', shape='plaintext')

dot.node("Reportes", '''<<TABLE BORDER="1" CELLBORDER="1" CELLSPACING="0">
  <TR><TD BGCOLOR="lightblue"><B>Reportes</B></TD></TR>
  <TR><TD>+ IdReporte : int</TD></TR>
  <TR><TD>+ IdUsuario : int</TD></TR>
  <TR><TD>+ FechaReporte : DateTime</TD></TR>
  <TR><TD>+ TotalImpactos : int</TD></TR>
</TABLE>>''', shape='plaintext')

# Nodos para los controladores
dot.node("ExamenController", "ExamenController", shape="box")
dot.node("HomeController", "HomeController", shape="box")
dot.node("ReportesController", "ReportesController", shape="box")
dot.node("TiradorController", "TiradorController", shape="box")
dot.node("UsuariosController", "UsuariosController", shape="box")

# Relación de dependencias entre controladores y ApplicationDbContext
dot.node("ApplicationDbContext", "ApplicationDbContext", shape="box")
dot.edge("ExamenController", "ApplicationDbContext")
dot.edge("ReportesController", "ApplicationDbContext")
dot.edge("TiradorController", "ApplicationDbContext")
dot.edge("UsuariosController", "ApplicationDbContext")

# Relación de los controladores con los modelos
dot.edge("ExamenController", "ExamenConfiguracion", label="Usa")
dot.edge("ReportesController", "Reportes", label="Usa")
dot.edge("ReportesController", "ImpactosBala", label="Usa")
dot.edge("UsuariosController", "Usuario", label="Usa")
dot.edge("UsuariosController", "ExternalUser", label="Usa")
dot.edge("TiradorController", "Usuario", label="Usa")

# Guardar el diagrama extendido
dot.render('/mnt/data/Diagrama_MVC_Controladores', format='png', cleanup=True)
'/mnt/data/Diagrama_MVC_Controladores.png'
