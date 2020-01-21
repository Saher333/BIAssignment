import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../header/header.service';
import { DataAccessService } from '../shared/data-access.service';
import { Employee } from '../shared/employee.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: any;

  constructor(private headerService: HeaderService, private dataAccess: DataAccessService, private http: HttpClient) { }

  ngOnInit() {
    this.headerService.setTitle('- List Employees');
    this.FetchEmployee();
  }

  private FetchEmployee() {
    this.employees = this.dataAccess.getEmployees();
  }

  onDelete(ID: number) {
    if (confirm("Are you sure do you want to delete this record?")) {
      this.dataAccess.deleteEmployee(ID).subscribe(
        (response) => {
          this.FetchEmployee();
          console.log(response);
        }
      )
    }
  }
}
