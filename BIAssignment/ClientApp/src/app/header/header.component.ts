import { Component, OnInit, AfterViewInit } from '@angular/core';
import { HeaderService } from './header.service';
import { DataAccessService } from '../shared/data-access.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, AfterViewInit {
  title = '';
  style: any;

  constructor(private headerService: HeaderService, private dataAccess: DataAccessService) { }

  ngOnInit() {
    this.headerService.title.subscribe(title => {
      this.title = title;
    });
  }

  ngAfterViewInit(): void {
    this.dataAccess.getConfig().subscribe(
      (data) => {
        this.style = JSON.parse(data['configData']);
      }
    );
  }
}
