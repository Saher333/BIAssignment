import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from '../shared/employee.model';
import { DataAccessService } from '../shared/data-access.service';
import { Benefit } from '../shared/benefit.model';
import { HeaderService } from '../header/header.service';

@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit {
  id: number;
  editMode = false;
  employeeForm: FormGroup;
  benefits: Benefit[];

  constructor(private route: ActivatedRoute, private dataAccess: DataAccessService, private headerService: HeaderService, private router: Router) { }

  ngOnInit() {
    this.route.params.subscribe(
      (params) => {
        this.id = +params['id'];
        this.editMode = params['id'] != null;

        if (this.editMode) {
          this.headerService.setTitle('- Edit Employee');
        } else {
          this.headerService.setTitle('- Add Employee');
        }

        this.initForm();        
      });
  }

  private initForm() {

    let benefitArr = new FormArray([]);
    this.employeeForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'dob': new FormControl(null, Validators.required),
      'salary': new FormControl(null, [Validators.required, Validators.pattern(/^[1-9]+[0-9]*$/)]),
      'benefits': benefitArr
    });

    this.dataAccess.getBenefits().subscribe(
      (benefits) => {
        this.benefits = benefits;

        for (let benefit of this.benefits) {
          benefitArr.push(new FormGroup({ 'included': new FormControl(false) }));
        }
        this.employeeForm.setControl('benefits', benefitArr);

        if (this.editMode) {
          this.dataAccess.getEmployee(this.id)
            .subscribe(
              (employee) => {
                let patchedArr = new FormArray([]);
                let benArr = this.getNames(this.benefits);
                let empBenArr = this.getNames(employee.benefits);
                for (let benefit of benArr) {
                  patchedArr.push(new FormGroup({ 'included': new FormControl(empBenArr.indexOf(benefit) != -1) }));
                }
                this.employeeForm.patchValue({
                  name: employee.name,
                  dob: employee.dob.toString().substring(0, 10),
                  salary: employee.salary
                });
                this.employeeForm.setControl('benefits', patchedArr);
              });
        }
      });
  }

  private getNames(benefits: Benefit[]) {
    let arr = [];
    for (let benefit of benefits) {
      arr.push(benefit.name);
    }
    return arr;
  }

  onSubmit() {
    let benefits: Benefit[] = [];
    let arr: any[] = this.employeeForm.value.benefits;
    for (var i = 0; i < arr.length; i++) {

      if (arr[i].included) {
        benefits.push(this.benefits[i]);
      }
    }
    let employee: Employee = {
      employeeId: 0,
      name: this.employeeForm.value.name,
      dob: this.employeeForm.value.dob,
      salary: this.employeeForm.value.salary,
      benefits: benefits
    };

    if (this.editMode) {
      employee.employeeId = this.id;
      this.dataAccess.updateEmployee(this.id, employee).subscribe(
        (response) => {
          console.log(response);
          this.router.navigate(['/employees']);
        }
      );
    } else {
      this.dataAccess.addEmployee(employee).subscribe(
        (response) => {
          console.log(response)
          this.router.navigate(['/employees']);
        }
      );
    }
  }
}
