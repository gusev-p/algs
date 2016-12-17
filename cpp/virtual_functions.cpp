#include <iostream>
#include <string>
#include <vector>
#include <sstream>

using namespace std;

class Person {
public:
	Person(int id) 
		:cur_id_(id) {
	}

	virtual void getdata() {
		cin >> name_;
		cin >> age_;
	};

	virtual void putdata() {
		cout << name_ << " " << age_;
	}

protected:
	string name_;
	int age_;
	int cur_id_;
};

class Student: public Person {
public:
	Student() 
		:Person(++last_id_) {
	}
	virtual void getdata() {
		Person::getdata();
		int len = sizeof(marks_) / sizeof(int);
		for(int i = 0; i < len; i++) {
			cin >> marks_[i];
		}
	}

	virtual void putdata() {
		Person::putdata();
		int sum = 0;
		int len = sizeof(marks_) / sizeof(int);
		for(int i = 0; i < len; i++)
			sum += marks_[i];
		cout << " " << sum << " " << cur_id_ << endl;
	}
private:
	int marks_[6];
	static int last_id_;
};
int Student::last_id_ = 0;

class Professor: public Person {
public:
	Professor() 
		:Person(++last_id_) {
	}
	virtual void getdata() {
		Person::getdata();
		std::string line;
		getline(cin, line);
		istringstream linestream(line);
		int pub;
		while(linestream >> pub)
			publications_.push_back(pub);
	}

	virtual void putdata() {
		Person::putdata();
		for(int i = 0; i < publications_.size(); i++) {
			cout << " " << publications_[i];
		}
		cout << " " << cur_id_ << endl;
	}
private:	
	vector<int> publications_;
	static int last_id_;
};
int Professor::last_id_ = 0;

int main() {
	int n;
	cin >> n;
	Person** persons = new Person*[n];
	for(int i = 0; i < n; i++) {
		int type;
		cin >> type;
		persons[i] = type == 1 ? dynamic_cast<Person*>(new Professor()) : new Student();
		persons[i]->getdata();
	}
	for(int i = 0; i < n; i++) {
		persons[i]->putdata();		
	}
	for(int i = 0; i < n; i++) {
		delete persons[i];
	}
	delete[] persons;
	return 0;
}