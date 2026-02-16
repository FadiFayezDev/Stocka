#!/usr/bin/env python3
"""
PostgreSQL Migration Validator
????? ?? ????? ???? Migrations ?? PostgreSQL
"""

import re
import sys
from pathlib import Path

class MigrationValidator:
    def __init__(self, migration_path):
        self.migration_path = Path(migration_path)
        self.issues = []
        self.warnings = []
    
    def validate_file(self, file_path):
        """??? ??? Migration ????"""
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        file_name = file_path.name
        print(f"\n?? ???: {file_name}")
        
        # 1. ?????? ?? GETUTCDATE()
        if 'GETUTCDATE()' in content:
            self.issues.append(f"? {file_name}: ??????? GETUTCDATE() ????? ?? NOW()")
        
        # 2. ?????? ?? SQL Server syntax ??? filters
        if '[UserId] IS NOT NULL' in content:
            self.issues.append(f"? {file_name}: ??????? SQL Server syntax ??? filter")
        
        # 3. ?????? ?? ApplicationUserId ?? BrandMemberships
        if 'ApplicationUserId' in content and 'BrandMemberships' in content:
            self.issues.append(f"? {file_name}: ApplicationUserId ????? ?? BrandMemberships")
        
        # 4. ?????? ?? defaultValue ?? 0m
        if 'defaultValue: 0m' in content:
            self.warnings.append(f"?? {file_name}: ??????? 0m ????? ?? 0")
        
        # 5. ?????? ?? ??? ???? Indexes ??? Date columns
        if 'PurchaseDate' in content and 'CreateIndex' not in content:
            self.warnings.append(f"?? {file_name}: ?? ???? index ??? PurchaseDate")
        
        if 'OrderDate' in content and 'CreateIndex' not in content:
            self.warnings.append(f"?? {file_name}: ?? ???? index ??? OrderDate")
        
        # 6. ?????? ?? ReferentialAction ???????
        cascade_count = content.count('ReferentialAction.Cascade')
        restrict_count = content.count('ReferentialAction.Restrict')
        
        if cascade_count == 0 and restrict_count == 0:
            self.warnings.append(f"?? {file_name}: ?? ???? delete behaviors ????? ?????")
    
    def validate_all(self):
        """??? ???? ????? Migrations"""
        print("=" * 60)
        print("?? PostgreSQL Migration Validator")
        print("=" * 60)
        
        migration_files = list(self.migration_path.glob("*InitialCreate.cs"))
        
        if not migration_files:
            print("? ?? ??? ?????? ??? ????? Migrations")
            return False
        
        for migration_file in sorted(migration_files):
            self.validate_file(migration_file)
        
        self.print_report()
        return len(self.issues) == 0
    
    def print_report(self):
        """????? ????? ???????"""
        print("\n" + "=" * 60)
        print("?? ???????")
        print("=" * 60)
        
        if self.issues:
            print(f"\n?? ??????? ?????? ({len(self.issues)}):")
            for issue in self.issues:
                print(f"  {issue}")
        else:
            print("\n? ?? ???? ????? ????")
        
        if self.warnings:
            print(f"\n?? ????????? ({len(self.warnings)}):")
            for warning in self.warnings:
                print(f"  {warning}")
        else:
            print("\n? ?? ???? ???????")
        
        print("\n" + "=" * 60)
        if self.issues:
            print("? ?????? ??? - ???? ??????? ?????")
            sys.exit(1)
        else:
            print("? ?????? ???!")
            sys.exit(0)

def main():
    migration_path = Path("./Infrastructure/Migrations")
    
    if not migration_path.exists():
        print("? ???? Migrations ??? ????")
        sys.exit(1)
    
    validator = MigrationValidator(migration_path)
    validator.validate_all()

if __name__ == "__main__":
    main()
