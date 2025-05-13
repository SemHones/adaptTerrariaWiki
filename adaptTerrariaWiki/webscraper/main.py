from scrapeData import ScrapeData

sd = ScrapeData()

url1 = "https://terraria.fandom.com/wiki/Ranged_weapons"
url2 = "https://terraria.fandom.com/wiki/Melee_weapons"
def main():
    # Melee class
    # sd.ScrapeAllTablesFromTerSite(url2, "Melee armor")


    # Ranged class
    sd.ScrapeAllTablesFromTerSite(url1, "Ranged weapon")
    sd.ScrapeAllTablesFromTerSite(url1, "Ranged armor")
    sd.ScrapeAllTablesFromTerSite(url1, "Ranged accessory")

main()
