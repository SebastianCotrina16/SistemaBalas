import xml.etree.ElementTree as ET

input_file = "reports/test_results.trx"
output_file = "reports/test_results.html"

tree = ET.parse(input_file)
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
    </style>
</head>
<body>
    <h1>Reporte de Pruebas</h1>
    <table>
        <tr>
            <th>Nombre de la Prueba</th>
            <th>Resultado</th>
            <th>Duración (s)</th>
        </tr>
"""

namespace = {"": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
for unit_test in root.findall(".//UnitTestResult", namespace):
    test_name = unit_test.attrib.get("testName", "N/A")
    outcome = unit_test.attrib.get("outcome", "N/A")
    duration = unit_test.attrib.get("duration", "N/A")

    html += f"""
    <tr>
        <td>{test_name}</td>
        <td>{outcome}</td>
        <td>{duration}</td>
    </tr>
    """


html += """
    </table>
</body>
</html>
"""

with open(output_file, "w", encoding="utf-8") as file:
    file.write(html)

print(f"Reporte HTML generado: {output_file}")
