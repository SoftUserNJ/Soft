export class Products{
    Code = "";
    ProductName = "";
    Pcs = "";
    MaxRate = "";
    Uom = "";
    Category = "";
    Brand = "";
    Country = "";
    Image = "";
    Description = "";
    BARCODE = "";
    InActive = "";
    discount = "";
}
export class ProductCreate{
    code  = "";
    name = "";
    description = "";
    barCode = "";
    uom = 0;
    category = 0;
    brand = 0;
    madeIn = 0;
    packing = 0;
    minRate = 0;
    maxRate = 0;
    purchaseRate = 0;
    retailPrice = 0;
    saleTax = 0;
    discount = 0;
    location  = 0;
    minimumLevel = 0;  
    status: boolean = false;
    dtNow = "";
}
 


export class Category{
    
    id = "";
    name = "";
    expiryDays = "";
}

export class Brand{
    
    id = "";
    name = "";
}

export class UOM{
    
    id = "";
    name = "";
}

export class MadeIN{
    
    id = "";
    name = "";
}