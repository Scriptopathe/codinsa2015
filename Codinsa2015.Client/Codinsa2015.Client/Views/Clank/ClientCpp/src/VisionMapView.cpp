#include "../inc/VisionMapView.h"
void VisionMapView::serialize(std::ostream& output) {
	// Vision
	output << this->Vision.size() << '\n';
	for(int Vision_it = 0; Vision_it < this->Vision.size(); Vision_it++) {
		output << this->Vision[Vision_it].size() << '\n';
		for(int VisionVision_it_it = 0; VisionVision_it_it < this->Vision[Vision_it].size(); VisionVision_it_it++) {
			output << ((int)this->Vision[Vision_it][VisionVision_it_it]) << '\n';
		}
	}

}

VisionMapView VisionMapView::deserialize(std::istream& input) {
	VisionMapView _obj = VisionMapView();
	// Vision
	std::vector<std::vector<VisionFlags>> _obj_Vision = std::vector<std::vector<VisionFlags>>();
	int _obj_Vision_count; input >> _obj_Vision_count; input.ignore(1000, '\n');
	for(int _obj_Vision_i = 0; _obj_Vision_i < _obj_Vision_count; _obj_Vision_i++) {
		std::vector<VisionFlags> _obj_Vision_e = std::vector<VisionFlags>();
		int _obj_Vision_e_count; input >> _obj_Vision_e_count; input.ignore(1000, '\n');
		for(int _obj_Vision_e_i = 0; _obj_Vision_e_i < _obj_Vision_e_count; _obj_Vision_e_i++) {
			int _obj_Vision_e_e_asInt; input >> _obj_Vision_e_e_asInt; input.ignore(1000, '\n');
			VisionFlags _obj_Vision_e_e = (VisionFlags)_obj_Vision_e_e_asInt;
			_obj_Vision_e.push_back((VisionFlags)_obj_Vision_e_e);
		}
		_obj_Vision.push_back((std::vector<VisionFlags>)_obj_Vision_e);
	}

	_obj.Vision = (::std::vector<std::vector<VisionFlags>>)_obj_Vision;
	return _obj;
}

VisionMapView::VisionMapView() {
}


