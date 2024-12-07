from xml.etree import ElementTree as ET

xml_file = 'test-reports/TEST-FaceRecognitionTestCase-20241115224445.xml' 
tree = ET.parse(xml_file)
root = tree.getroot()

html = """
<html>
<head>
    <title>Reporte de Pruebas</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid black; padding: 8px; text-align: left; }
        th { background-color: #f2f2f2; }
        .success { color: green; }
        .failure { color: red; }
    </style>
</head>
<body>
    <h1>Reporte de Pruebas</h1>
    <table>
        <tr>
            <th>Clase</th>
            <th>Prueba</th>
            <th>Tiempo (s)</th>
            <th>Estado</th>
            <th>Salida</th>
            <th>Error</th>
        </tr>
"""


for testcase in root.findall("testcase"):
    classname = testcase.get("classname")
    name = testcase.get("name")
    time = testcase.get("time")
    system_out = testcase.find("system-out")
    system_err = testcase.find("system-err")

    if system_err is not None:
        status = '<span class="failure">Error</span>'
    else:
        status = '<span class="success">Éxito</span>'

    output = system_out.text.strip() if system_out is not None else "N/A"
    error = system_err.text.strip() if system_err is not None else "N/A"

    html += f"""
    <tr>
        <td>{classname}</td>
        <td>{name}</td>
        <td>{time}</td>
        <td>{status}</td>
        <td><pre>{output}</pre></td>
        <td><pre>{error}</pre></td>
    </tr>
    """

html += """
    </table>
</body>
</html>
"""

with open("report.html", "w", encoding="utf-8") as file:
    file.write(html)

print("Reporte HTML generado: report.html")
