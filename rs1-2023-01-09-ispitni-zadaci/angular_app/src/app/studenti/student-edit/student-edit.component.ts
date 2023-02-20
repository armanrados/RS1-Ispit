import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css']
})
export class StudentEditComponent implements OnInit {

  @Input() ime:string;
  @Input() prezime:string;
  @Input() odabranaOpstina:string;
  @Input() opstine:any;
  @Input() studentId: number;
  @Output() otvori = new EventEmitter<boolean>();
  show = true;
  constructor(private httpKlijent: HttpClient) { }

  ngOnInit(): void {
  }

  zatvori()
  {
    this.show = !this.show;
    this.otvori.emit(this.show);
  }

  spasiPromjene()
  {
    let opstinaId = 0;
    this.opstine.forEach(
      (opstina:any) => {
        if(opstina.opis == this.odabranaOpstina)
          opstinaId = opstina.id;
      }
    );
    let izmijenjenStudent =
      {
        ime : this.ime,
        prezime: this.prezime,
        studentId: this.studentId,
        opcinaId : opstinaId
      }
      this.httpKlijent.put(MojConfig.adresa_servera+"/UrediStudenta", izmijenjenStudent, MojConfig.http_opcije())
        .subscribe(
          () =>
          {
              porukaSuccess("Uspjesno izmijenjen student!");
              window.location.reload();
          }
        );
  }
}
