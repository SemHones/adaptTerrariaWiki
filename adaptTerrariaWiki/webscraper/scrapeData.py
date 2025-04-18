from urllib.request import urlopen
from bs4 import BeautifulSoup
import json
import base64
import requests
import re

class ScrapeData:
    def ScrapeAllTablesFromTerSite(self, url, tableTitle):
        page = urlopen(url)
        html = page.read().decode("utf-8")
        soup = BeautifulSoup(html, "html.parser")
        tables = soup.find_all("table", class_=["sortable", "lined"])

        jsonTables = []

        count = 1
        for table in tables:
            if table:
                
                newTitle = tableTitle + " " + str(count)
                
                tempJson = self.loadingTables(table, newTitle)
                if tempJson["contents"]:
                    jsonTables.append(tempJson)
                
                count += 1

                if(tempJson["tableTitle"] == "Empty"):
                    break # This is a temporary fix, but it works for now, i promise... :)
            else:
                print("Table not found.")
        print(json.dumps(jsonTables, indent=4))

    def loadingTables(self, table, tableTitle):
        # Extract text from table
        table = (table.encode('utf-8'))
        soup = BeautifulSoup(table, "html.parser")
        table = soup.find('table')

        data = {
            "tableTitle": tableTitle,
            "contents": []
        }
        # Indexes are purely for debugging purposes
        armorIndex = 0
        weaponIndex = 0
        accessoryIndex = 0
        for row in table.find_all('tr')[1:]:  
            
            row_data = {}
            
            link_tag = row.find('a')
            cells = row.find_all(['th', 'td'])
            if(link_tag):
                # print("data")
                # print(data)

                if "armor" in tableTitle:
                    data = self.returnJsonOfArmorTable(link_tag, row_data, cells, data, armorIndex)
                    armorIndex += 1
                elif "weapon" in tableTitle:
                    data = self.returnJsonOfWeaponTable(link_tag, row_data, cells, data, weaponIndex)
                    # This only works for weapons, since they are the first tables displayed and the next table found this way is armor an it crashes on accessories.
                    # This is a temporary fix, but it works for now. :)
                    if(data["contents"] == []):
                        print("All weapons scraped... probably, fix your code if it wasn't all weapons")
                        print("no weapon found, stopping")
                        data['tableTitle'] = "Empty" # This makes sure the next tables are not scraped
                    weaponIndex += 1
                elif "accessory" in tableTitle:
                    data = self.returnJsonOfAccessoryTable(link_tag, row_data, cells, data, accessoryIndex)
                    accessoryIndex += 1
        return data

    # also loads buffs and debuffs 
    def returnJsonOfAccessoryTable(self, link_tag, row_data, cells, data, accessoryIndex):
        if cells is None:
            return data
        
        if isinstance(cells, list):
            if(len(cells) >= 4): # get only small tables since accessories have 4 columns
                return data

        if link_tag:
            first_image = link_tag.find('img')
            print("first_image")
            print(first_image)
            if first_image:
                row_data['name'] = first_image['alt']
                if "http" in first_image['src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['src'])
                elif "http" in first_image['data-src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
            row_data['href'] = link_tag['href']
        row_data['boost'] = self.filterString(cells[2].get_text(strip=True))

        # Append the row data to the list
        data["contents"].append(row_data)
        return data
    
    def returnJsonOfArmorTable(self, link_tag, row_data, cells, data, armorIndex):
        if link_tag:
            first_image = link_tag.find('img')
            if first_image:
                # melee should work with just data-src, but ranged for some reason doesn't have the data-src attribute (make the insanity stop)
                if 'data-src' not in first_image:
                    if not "armor" in first_image['alt']:
                        print("Not an armor")
                        return data
                else:
                    if not "armor" in first_image['data-src']:
                        print("Not an armor")
                        return data
                # make the print work
                print("Armor found " + str(armorIndex))

                if "http" in first_image['src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['src'])
                elif "http" in first_image['data-src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
            row_data['href'] = link_tag['href']
            
        row_data['name'] = cells[1].get_text(strip=True)
        row_data['head'] = cells[2].get_text(strip=True)
        row_data['chest'] = cells[3].get_text(strip=True)
        row_data['legs'] = cells[4].get_text(strip=True)
        row_data['bonus'] = self.extract_bonus_text(cells[6])
        if("Head:" in cells[7].get_text(strip=True)):
            row_data['obtained_by'] = "Crafted"
        else:
            row_data['obtained_by'] = self.extract_obtained_by(cells[7])

        # Append the row data to the list
        data["contents"].append(row_data)
        return data
            
    def returnJsonOfWeaponTable(self, link_tag, row_data, cells, data, weaponIndex):
        
        if link_tag:
            first_image = link_tag.find('img')
            if first_image:
                # check to make sure only weapons are being scraped
                if "armor" in first_image['alt']:
                    print("Not an weapon")
                    return data
                # print("Weapon found " + str(weaponIndex))
                row_data['name'] = first_image['alt']
                if "http" in first_image['src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['src'])
                elif "http" in first_image['data-src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
            row_data['href'] = link_tag['href']
        row_data['damage'] = cells[3].get_text(strip=True)
        if("Crafted" in cells[4].get_text(strip=True)):
            row_data['obtained_by'] = "Crafted"
        else:
            row_data['obtained_by'] = self.extract_obtained_by(cells[4])
        row_data['notes'] = self.filterString(cells[5].get_text())

        # Append the row data to the list
        data["contents"].append(row_data)
        return data
    
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
        
    def extract_bonus_text(self, cell):
        soup = BeautifulSoup(str(cell), 'html.parser')
        bonuses = []

        for li in soup.find_all('li'):
            span = li.find('span')
            
            # Als er geen <span> is, of de span heeft GEEN title met "PC", skip
            if span and "PC" not in span.get("title", ""):
                continue

            # Tekst uit de li zelf (zonder span-elementen)
            # .get_text() haalt alles op (ook nested), maar strip() maakt het netter
            li_text = li.get_text(strip=True)
            bonuses.append(li_text)

        return "\n".join(bonuses)
    
    def extract_obtained_by(self, html):
        soup = BeautifulSoup(str(html), 'html.parser')
        result = []

        td = soup.find('td', class_='small')
        if not td:
            return ""

        li_elements = td.find_all('li')
        for li in li_elements:
            line = ''.join(part.strip() for part in li.stripped_strings)
            result.append(line)

        return "\n".join(result)