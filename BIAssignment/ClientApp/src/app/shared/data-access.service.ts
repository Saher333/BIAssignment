import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './employee.model';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs/Observable';
import { Benefit } from './benefit.model';

@Injectable()
export class DataAccessService {
  url = 'https://localhost:44331/';

  constructor(private httpClient: HttpClient) { }

  getBenefits(): Observable<Benefit[]> {
    return this.httpClient.get<Benefit[]>(this.url + 'api/Benefits');
  }

  getEmployees() {
    return this.httpClient.get(this.url + 'api/Employees');
  }

  getEmployee(id: number): Observable<Employee> {
    return this.httpClient.get<Employee>(this.url + 'api/Employees/' + id);
  }

  addEmployee(employee: Employee) {
    return this.httpClient.post(this.url + 'api/Employees', employee);
  }

  updateEmployee(id: number, employee: Employee) {
    return this.httpClient.put(this.url + 'api/Employees/' + id, employee);
  }

  deleteEmployee(id: number) {
    return this.httpClient.delete(this.url + 'api/Employees/' + id);
  }

  getConfig() {
    return this.httpClient.get(this.url + 'api/SystemConfigs');
  }
}
