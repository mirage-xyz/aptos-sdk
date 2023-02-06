:: 0) Before build docs make sure that all files with in-code documentation was mentioned in config file
@RD /S /Q ".\_site"
@RD /S /Q ".\obj"
:: 1) Generate metadata
docfx metadata
:: 2) Create docs
docfx build



