from urllib.request import urlopen
from bs4 import BeautifulSoup
import re
import json
import base64
import requests

class ScrapeData:
    def ScrapeAllTablesFromTerSite(self, url, tableTitle):
        page = urlopen(url)
        html = page.read().decode("utf-8")
        soup = BeautifulSoup(html, "html.parser")
        tables = soup.find_all("table", class_=["sortable", "lined"])
        # tables = soup.find_all("table", class_="sortable")

        jsonTables = []

        count = 1
        for table in tables:
            if table:
                
                newTitle = tableTitle + " " + str(count)
                if "armor" in tableTitle:
                    tempJson = self.returnJsonOfArmorTable(table, newTitle)
                    if tempJson["contents"]:
                        jsonTables.append(tempJson)
                elif "weapon" in tableTitle:
                    tempJson = self.returnJsonOfWeaponTable(table, newTitle)
                    if tempJson["contents"]:
                        jsonTables.append(tempJson)
                count += 1
            else:
                print("Table not found.")
        print(json.dumps(jsonTables, indent=4))

    def ScrapeFirstTableFromTerSite(self, url):
        page = urlopen(url)
        html = page.read().decode("utf-8")
        soup = BeautifulSoup(html, "html.parser")
        table = soup.find("table")

        if table:
            self.returnJsonOfWeaponTable(table)
        else:
            print("Table not found.")


    def returnJsonOfWeaponTable(self, table, tableTitle):
        # Extract text from table
        table = (table.encode('utf-8'))
        soup = BeautifulSoup(table, "html.parser")
        table = soup.find('table')

        data = {
            "tableTitle": tableTitle,
            "contents": []
            }
        for row in table.find_all('tr')[1:]:  
            row_data = {}
            
            link_tag = row.find('a')
            cells = row.find_all(['th', 'td'])
            if len(cells) < 6:
                break
            if link_tag:
                first_image = link_tag.find('img')
                if first_image:
                    # check to make sure only weapons are being scraped
                    if "armor" in first_image['alt']:
                        break
                    row_data['name'] = first_image['alt']
                    # print(first_image['src'])
                    # print(first_image['data-src'])
                    if "http" in first_image['src']:
                        row_data['image'] = self.convertUrlTobase64(first_image['src'])
                    elif "http" in first_image['data-src']:
                        row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
                row_data['href'] = link_tag['href']
            row_data['damage'] = cells[3].get_text(strip=True)
            if("Crafted" in cells[4].get_text(strip=True)):
                row_data['obtained_by'] = "Crafted"
            else:
                
                row_data['obtained_by'] = self.filterString(cells[4].get_text())
            row_data['notes'] = self.filterString(cells[5].get_text())

            # Append the row data to the list
            data["contents"].append(row_data)
            
        return data
        # print(json.dumps(data, indent=4))

    def returnJsonOfArmorTable(self, table, tableTitle):
        # Extract text from table
        table = (table.encode('utf-8'))
        soup = BeautifulSoup(table, "html.parser")
        table = soup.find('table')

        data = {
            "tableTitle": tableTitle,
            "contents": []
            }
        for row in table.find_all('tr')[1:]:  
            row_data = {}
            
            link_tag = row.find('a')
            cells = row.find_all(['th', 'td'])
            if len(cells) < 6:
                print("Cells length is less than 6")
                break
            if link_tag:
                first_image = link_tag.find('img')
                if first_image:
                    # check to make sure only armors are being scraped
                    if not "armor" in first_image['alt']:
                        print("Not an armor")
                        break
                    print("Armor found")
                    row_data['name'] = first_image['alt']
                    if "http" in first_image['src']:
                        row_data['image'] = self.convertUrlTobase64(first_image['src'])
                    elif "http" in first_image['data-src']:
                        row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
                row_data['href'] = link_tag['href']
            if("Head:" in cells[2].get_text(strip=True)):
                row_data['obtained_by'] = "Crafted"
            else:
                row_data['obtained_by'] = self.filterString(cells[2].get_text())
                
            row_data['head'] = cells[3].get_text(strip=True)
            row_data['chest'] = cells[4].get_text(strip=True)
            row_data['legs'] = cells[5].get_text(strip=True)
            row_data['sum'] = cells[6].get_text(strip=True)
            row_data['bonus'] = self.filterString(cells[7].get_text())

            # Append the row data to the list
            data["contents"].append(row_data)
            
        return data
        # print(json.dumps(data, indent=4))

    def filterString(self, string):
        arrayExceptions = ["\u00a0", "\n"]
        for exception in arrayExceptions:
            string = string.replace(exception, "")
        return string
    
    # slow af yo
    def convertUrlTobase64(self, url):
        try:
            response = requests.get(url)
            if response.status_code == 200:
                # Convert image content to base64-encoded string
                base64_str = base64.b64encode(response.content).decode('utf-8')
                # print("Image converted to base64")
                data_url = f"data:image/jpeg;base64,{base64_str}"
                # print(data_url)
                return data_url
            else:
                print("Error:", response.status_code)
                return url
        except Exception as e:
            print("Error:", e)
            return url