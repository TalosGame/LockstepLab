#!/bin/sh
echo "Generate c2s proto"

tools_real_path="${PWD%/*/*}"
common_dir="common"
c2s_dir="c2s"

protogen_csharp_bin_path="${tools_real_path}/bin/protobuf_net"
protobuf_v2_bin_path="${tools_real_path}/bin/protobuf_v2/mac"

protos_path="${tools_real_path}/protos"
common_proto_path="${protos_path}"
c2s_proto_path="${protos_path}/c2s"

out_csharp_path="${tools_real_path}/out/c2s/c#"
out_java_path="${tools_real_path}/out/c2s/java"

temp_proto_csharp_files_dir="${protogen_csharp_bin_path}/temp"
temp_proto_java_file_dir="${protobuf_v2_bin_path}/temp"

#拷贝protos文件
echo "copy proto files"
mkdir -p ${temp_proto_csharp_files_dir}
mkdir -p ${temp_proto_java_file_dir}

filelist=`ls ${common_proto_path}/*.proto`
for file in $filelist
do
	echo "common proto file:${file}"
	cp $file ${temp_proto_csharp_files_dir}
	cp $file ${temp_proto_java_file_dir}
done

filelist=`ls ${c2s_proto_path}/*.proto`
for file in $filelist
do
	echo "c2s proto file:${file}"
	cp $file ${temp_proto_csharp_files_dir}
	cp $file ${temp_proto_java_file_dir}
done

filelist=`ls ${temp_proto_csharp_files_dir}/*.proto`
for file in $filelist
do
	#获取文件相对路径
	file_full_name=${file##*/}
	file_name=${file_full_name%.*}
	echo "proto file full name:${file_full_name}"
	echo "proto file name:${file_name}"

	#生成c#文件.这里必须绝对路径!
	mono ${protogen_csharp_bin_path}/protogen.exe -i:$file -o:${out_csharp_path}/${file_name}.cs

	#生成java文件
	${protobuf_v2_bin_path}/protoc --proto_path=${temp_proto_java_file_dir} --java_out=${out_java_path} ${temp_proto_java_file_dir}/${file_full_name}
done

#删除临时文件夹
rm -rf ${temp_proto_csharp_files_dir}
rm -rf ${temp_proto_java_file_dir}
