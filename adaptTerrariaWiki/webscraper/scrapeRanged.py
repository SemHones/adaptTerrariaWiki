from urllib.request import urlopen
from bs4 import BeautifulSoup
import base64
import requests

class ScrapeRanged:

    def returnJsonOfAccessoryTable(self, link_tag, row_data, cells, data, accessoryIndex):
        if cells is None:
            return data
        
        if isinstance(cells, list):
            if(len(cells) >= 4): # get only small tables since accessories have 4 columns
                return data

        if link_tag:
            first_image = link_tag.find('img')
            if first_image:
                row_data['name'] = first_image['alt']
                if "http" in first_image['src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['src'])
                elif "http" in first_image['data-src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
            row_data['href'] = link_tag['href']
        row_data['boost'] = self.extract_bonus_text(cells[2])

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
                        # print("Not an armor") # this stays here for debugging
                        return data
                else:
                    if not "armor" in first_image['data-src']:
                        # print("Not an armor") # this stays here for debugging
                        return data

                if "http" in first_image['src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['src'])
                elif "http" in first_image['data-src']:
                    row_data['image'] = self.convertUrlTobase64(first_image['data-src'])
            row_data['href'] = link_tag['href']
            
        row_data['name'] = cells[1].get_text(strip=True)
        row_data['head'] = cells[3].get_text(strip=True)
        row_data['chest'] = cells[4].get_text(strip=True)
        row_data['legs'] = cells[5].get_text(strip=True)
        row_data['bonus'] = self.extract_bonus_text(cells[7])
        if("Head:" in cells[2].get_text(strip=True)):
            row_data['obtained_by'] = "Crafted"
        else:
            row_data['obtained_by'] = self.extract_obtained_by(cells[2])

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
            if "Set Bonus:" in li.get_text(strip=True):
                # Dit is de set bonus, dus we willen deze wel
                bonuses.append(li.get_text(strip=True))
                continue
            
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