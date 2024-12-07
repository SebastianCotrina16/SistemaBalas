from xml.etree import ElementTree as ET

xml_file = 'test-reports/TEST-BulletDetectionTestCase-20241115225532.xml'
tree = ET.parse(xml_file)
root = tree.getroot()

# Construcción inicial del HTML
html = """
<html>
<head>
    <title>Reporte de Pruebas</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid black; padding: 8px; text-align: left; }}
        th {{ background-color: #f2f2f2; }}
        .success {{ color: green; }}
        .failure {{ color: red; }}
    </style>
</head>
<body>
    <h1>Reporte de Pruebas</h1>
    <h2>Suite: {suite_name}</h2>
    <p>Total de pruebas: {total_tests}</p>
    <p>Tiempo total: {total_time} segundos</p>
    <p>Fallos: {failures}</p>
    <p>Errores: {errors}</p>
    <table>
        <tr>
            <th>Clase</th>
            <th>Prueba</th>
            <th>Tiempo (s)</th>
        </tr>
""".format(
    suite_name=root.get("name"),
    total_tests=root.get("tests"),
    total_time=root.get("time"),
    failures=root.get("failures"),
    errors=root.get("errors")
)

# Procesar cada prueba en el XML
for testcase in root.findall("testcase"):
    classname = testcase.get("classname")
    name = testcase.get("name")
    time = testcase.get("time")

    # Agregar la fila al HTML
    html += f"""
    <tr>
        <td>{classname}</td>
        <td>{name}</td>
        <td>{time}</td>
    </tr>
    """

# Finalizar el HTML
html += """
    </table>
</body>
</html>
"""

# Guardar el HTML en un archivo
output_file = "bullet_detection_report.html"
with open(output_file, "w", encoding="utf-8") as file:
    file.write(html)

print(f"Reporte HTML generado: {output_file}")